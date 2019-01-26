using System;

namespace PagingDemo.Models
{
    public class Collection<T> : Resource
    {
        public T[] Value { get; set; }
    }
}
