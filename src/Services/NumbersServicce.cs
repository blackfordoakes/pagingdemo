using System;
using System.Collections.Generic;
using System.Linq;
using PagingDemo.Models;

namespace PagingDemo.Services
{
    public class NumbersServicce : INumbersServicce
    {
        private const int ARRAY_SIZE = 100;

        public PagedResults<int> GetNumbers(PagingOptions pagingOptions)
        {
            // get the list we want to page
            // OK, this is a little silly, but it's for demo purposes
            List<int> values = new List<int>();

            for(int i = 1; i <= ARRAY_SIZE; i++)
            {
                values.Add(i);
            }

            // now, apply paging
            var pagedValues = values.Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value);

            // and return
            return new PagedResults<int>
            {
                Items = pagedValues,
                TotalSize = values.Count
            };
        }

        public PagedResults<int> GetRandomNumbers(PagingOptions pagingOptions)
        {
            List<int> values = new List<int>();
            Random rnd = new Random();

            for(int i = 1; i <=ARRAY_SIZE; i++)
            {
                values.Add(rnd.Next(ARRAY_SIZE));
            }

            // now, apply paging
            var pagedValues = values.Skip(pagingOptions.Offset.Value)
                .Take(pagingOptions.Limit.Value);

            // and return
            return new PagedResults<int>
            {
                Items = pagedValues,
                TotalSize = values.Count
            };
        }
    }
}
