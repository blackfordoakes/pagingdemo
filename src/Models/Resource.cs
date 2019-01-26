using System;
using Newtonsoft.Json;

namespace PagingDemo.Models
{
    public abstract class Resource : Link
    {
        [JsonIgnore]
        public Link Self { get; set; }
    }
}
