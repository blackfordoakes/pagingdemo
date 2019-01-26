using System;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;

namespace PagingDemo.Models
{
    public class PagedCollection<T> : Collection<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Offset { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Limit { get; set; }

        public int Size { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link First { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link Previous { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link Next { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Link Last { get; set; }

        public static PagedCollection<T> Create(Link self, T[] items, int size, PagingOptions pagingOptions)
            => Create<PagedCollection<T>>(self, items, size, pagingOptions);

        public static TResponse Create<TResponse>(Link self, T[] items, int size, PagingOptions pagingOptions)
            where TResponse : PagedCollection<T>, new()
            => new TResponse
            {
                Self = self,
                Value = items,
                Size = size,
                Offset = pagingOptions.Offset,
                Limit = pagingOptions.Limit,
                First = self,
                Next = _getNextLink(self, size, pagingOptions),
                Previous = _getPreviousLink(self, size, pagingOptions),
                Last = _getLastLink(self, size, pagingOptions)
            };

        private static Link _getNextLink(Link self, int size, PagingOptions options)
        {
            if (options?.Limit == null)
                return null;

            if (options?.Offset == null)
                return null;

            int limit = options.Limit.Value;
            int offset = options.Offset.Value;

            int nextPage = offset + limit;
            if (nextPage >= size)
                return null;

            var parameters = new RouteValueDictionary(self.RouteValues)
            {
                ["limit"] = limit,
                ["offset"] = nextPage
            };

            var newLink = Link.ToCollection(self.RouteName, parameters);
            return newLink;
        }

        private static Link _getPreviousLink(Link self, int size, PagingOptions options)
        {
            if (options?.Limit == null) return null;
            if (options?.Offset == null) return null;

            var limit = options.Limit.Value;
            var offset = options.Offset.Value;

            if (offset == 0)
            {
                return null;
            }

            if (offset > size)
            {
                return _getLastLink(self, size, options);
            }

            var previousPage = Math.Max(offset - limit, 0);

            if (previousPage <= 0)
            {
                return self;
            }

            var parameters = new RouteValueDictionary(self.RouteValues)
            {
                ["limit"] = limit,
                ["offset"] = previousPage
            };
            var newLink = Link.ToCollection(self.RouteName, parameters);

            return newLink;
        }

        private static Link _getLastLink(Link self, int size, PagingOptions options)
        {
            if (options?.Limit == null) return null;

            var limit = options.Limit.Value;

            if (size <= limit) return null;

            var offset = Math.Ceiling((size - (double)limit) / limit) * limit;

            var parameters = new RouteValueDictionary(self.RouteValues)
            {
                ["limit"] = limit,
                ["offset"] = offset
            };
            var newLink = Link.ToCollection(self.RouteName, parameters);

            return newLink;
        }
    }
}
