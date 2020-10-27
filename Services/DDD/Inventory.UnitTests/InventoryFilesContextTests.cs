using Inventory.Infrastructure.GroupVarsFiles;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests
{
    public class InventoryFilesContextTests
    {


        private ILogger<InventoryFilesContext> _logger;

        [SetUp]
        public void Setup()
        {
             _logger = new Logger<InventoryFilesContext>(new NullLoggerFactory());
        }



        [Test]
        public async Task GetVariablesTest()
        {
            var ctx = new InventoryFilesContext(@"D:\devel\github\ansible_inventory\tests\inventories\poc\group_vars", _logger);
            var variables = await ctx.GetVariablesAsync();

        }
    }
}
