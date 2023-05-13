using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.API.Application
{
    [TestFixture]
    public class ServerTest : BaseDbInventoryTests
    {

        private readonly ILogger<ServerTest> _logger;

        public ServerTest()
        {
            _logger = this.GetLogger<ServerTest>();

        }

        //[Test]
        //public async Task Should_successfull_create_server_with_valid_values()
        //{
        //    Assert.Fail("Not Yet Implemented");
        //}

    }
}
