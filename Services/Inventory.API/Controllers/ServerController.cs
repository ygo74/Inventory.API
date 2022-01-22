using Inventory.API.Application.Servers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {

        private readonly ILogger<ServerController> _logger;
        private readonly ServerService _serverService;

        public ServerController(ILogger<ServerController> logger, ServerService serverService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serverService = serverService ?? throw new ArgumentNullException(nameof(logger));
        }


        // GET: api/<ServerController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ServerController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ServerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ServerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
