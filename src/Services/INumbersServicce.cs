using System.Collections.Generic;
using PagingDemo.Models;

namespace PagingDemo.Services
{
    public interface INumbersServicce
    {
        PagedResults<int> GetNumbers(PagingOptions pagingOptions);

        PagedResults<int> GetRandomNumbers(PagingOptions pagingOptions);
    }
}