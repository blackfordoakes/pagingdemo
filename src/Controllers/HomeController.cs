using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PagingDemo.Models;
using PagingDemo.Services;

namespace PagingDemo.Controllers
{
    [Route("api/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly INumbersServicce _numbersService;
        private readonly PagingOptions _defaultPagingOptions;

        public HomeController(INumbersServicce numbersService, IOptions<PagingOptions> defaultPagingWrapper)
        {
            _numbersService = numbersService;
            _defaultPagingOptions = defaultPagingWrapper.Value;
        }

        [HttpGet("numbers", Name = nameof(GetNumbers))]
        [ProducesResponseType(200)]
        public ActionResult<Collection<int>> GetNumbers([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions = _defaultPagingOptions.Replace(pagingOptions);

            var numbers = _numbersService.GetNumbers(pagingOptions);

            var collection = PagedCollection<int>.Create<PagedCollection<int>>(
                Link.ToCollection(nameof(GetNumbers)),
                numbers.Items.ToArray(),
                numbers.TotalSize,
                pagingOptions);

            return collection;
        }

        [HttpGet("random", Name = nameof(GetRandomNumbers))]
        [ProducesResponseType(200)]
        public ActionResult<Collection<int>> GetRandomNumbers([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions = _defaultPagingOptions.Replace(pagingOptions);

            var numbers = _numbersService.GetRandomNumbers(pagingOptions);

            var collection = PagedCollection<int>.Create<PagedCollection<int>>(
                Link.ToCollection(nameof(GetRandomNumbers)),
                numbers.Items.ToArray(),
                numbers.TotalSize,
                pagingOptions);

            return collection;
        }
    }
}