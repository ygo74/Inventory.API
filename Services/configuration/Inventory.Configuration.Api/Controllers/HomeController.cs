using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Configuration.Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return new RedirectResult("~/graphql");
        }
    }
}
