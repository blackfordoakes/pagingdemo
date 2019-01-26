﻿using System;
using Microsoft.AspNetCore.Mvc;
using PagingDemo.Models;

namespace PagingDemo.Infrastructure
{
    public class LinkRewriter
    {
        private readonly IUrlHelper _urlHelper;

        public LinkRewriter(IUrlHelper helper)
        {
            _urlHelper = helper;
        }

        public Link Rewrite(Link original)
        {
            if (original == null)
                return null;

            return new Link
            {
                Href = _urlHelper.Link(original.RouteName, original.RouteValues),
                Method = original.Method,
                Relations = original.Relations
            };
        }
    }
}
