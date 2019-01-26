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
        public ActionResult<PagedResults<int>> GetNumbers([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions = _defaultPagingOptions.Replace(pagingOptions);

            var numbers = _numbersService.GetNumbers(pagingOptions);
            return numbers;
        }

        [HttpGet("random", Name = nameof(GetRandomNumbers))]
        [ProducesResponseType(200)]
        public ActionResult<PagedResults<int>> GetRandomNumbers([FromQuery] PagingOptions pagingOptions)
        {
            pagingOptions = _defaultPagingOptions.Replace(pagingOptions);

            var numbers = _numbersService.GetRandomNumbers(pagingOptions);
            return numbers;
        }
    }
}