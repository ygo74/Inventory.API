using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Inventory.API.Infrastructure;
using Inventory.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServersController : ControllerBase
    {

        private readonly InventoryContext _inventoryContext;
        private readonly ILogger<ServersController> _logger;

        public ServersController(InventoryContext context, ILogger<ServersController> logger)
        {
            _inventoryContext = context ?? throw new ArgumentNullException(nameof(context));
            _logger           = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogDebug("Start Controller");
        }

        // GET: api/<ServersController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    _logger.LogDebug("Get");
        //    return new string[] { "value1", "value2" };
        //}

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Server>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get()
        {
            _logger.LogDebug("Get");
            var servers = await _inventoryContext.Servers.ToListAsync();

            return Ok(servers);            
        }


        [HttpGet]
        [Route("{name:alpha}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(Server), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Server>> ServerByName(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var item = await _inventoryContext.Servers.SingleOrDefaultAsync(server => server.Name == name);

            if (item != null)
            {
                return item;
            }

            return NotFound();
        }

        //POST api/v1/[controller]/items
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateServerAsync([FromBody] Server server)
        {
            var newServer = new Server
            {
                Name = server.Name,
                OperatingSystem = server.OperatingSystem    
            };

            _inventoryContext.Servers.Add(newServer);

            await _inventoryContext.SaveChangesAsync();

            return CreatedAtAction(nameof(ServerByName), new { name = newServer.Name }, null);
        }

        // PUT api/<ServersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
