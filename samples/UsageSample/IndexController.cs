using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UsageSample
{
    public class IndexController : Controller
    {
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet]
        [Route("/exception")]
        public IActionResult Exception()
        {
            throw new Exception("Something");
        }
    }
}