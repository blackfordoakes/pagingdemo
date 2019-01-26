using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using PagingDemo.Infrastructure;
using PagingDemo.Models;

namespace PagingDemo.Filters
{
    public class LinkRewritingFilter : IAsyncResultFilter
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public LinkRewritingFilter(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var asObjectResult = context.Result as ObjectResult;
            bool shouldSkip = asObjectResult?.StatusCode > 400
                || asObjectResult?.Value == null
                || asObjectResult?.Value as Resource == null;

            if (shouldSkip)
                return next();

            var rewriter = new LinkRewriter(_urlHelperFactory.GetUrlHelper(context));
            _rewriteAllLinks(asObjectResult.Value, rewriter);

            return next();
        }

        private static void _rewriteAllLinks(object model, LinkRewriter rewriter)
        {
            if (model == null)
                return;

            var allProperties = model.GetType().GetTypeInfo()
                .GetAllProperties().Where(p => p.CanRead)
                .ToArray();

            var linkProperties = allProperties.Where(p => p.CanWrite && p.PropertyType == typeof(Link));

            foreach (var linkProperty in linkProperties)
            {
                var rewritten = rewriter.Rewrite(linkProperty.GetValue(model) as Link);

                if (rewritten != null)
                {
                    linkProperty.SetValue(model, rewritten);

                    // special handling of self property
                    if (linkProperty.Name == nameof(Resource.Self))
                    {
                        allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Href))
                            ?.SetValue(model, rewritten.Href);

                        allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Method))
                                                   ?.SetValue(model, rewritten.Method);

                        allProperties.SingleOrDefault(p => p.Name == nameof(Resource.Relations))
                                                   ?.SetValue(model, rewritten.Relations);
                    }
                }
            }

            // do recursive if in array
            var arrayProperties = allProperties.Where(p => p.PropertyType.IsArray);
            _rewriteArrays(arrayProperties, model, rewriter);

            // now do other object properties
            var objectProperties = allProperties.Except(arrayProperties).Except(linkProperties);
            _rewriteNestedObjects(objectProperties, model, rewriter);
        }

        private static void _rewriteNestedObjects(IEnumerable<PropertyInfo> objectProperties, object model, LinkRewriter rewriter)
        {
            foreach (var objectProperty in objectProperties)
            {
                if (objectProperty.PropertyType == typeof(string))
                {
                    continue;
                }

                var typeInfo = objectProperty.PropertyType.GetTypeInfo();
                if (typeInfo.IsClass)
                {
                    _rewriteAllLinks(objectProperty.GetValue(model), rewriter);
                }
            }
        }

        private static void _rewriteArrays(IEnumerable<PropertyInfo> arrayProperties, object model, LinkRewriter rewriter)
        {
            foreach (var arrayProperty in arrayProperties)
            {
                var array = arrayProperty.GetValue(model) as Array ?? new Array[0];

                foreach (var element in array)
                {
                    _rewriteAllLinks(element, rewriter);
                }
            }
        }
    }
}
