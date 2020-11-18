using Inventory.Infrastructure.GroupVarsFiles;
using Microsoft.Extensions.Caching.Memory;
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
        public void GetVariablesTest()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());

            var ctx = new InventoryFilesContext(memoryCache, _logger);
            var variables = ctx.GetGroupVariables(@"D:\devel\github\ansible_inventory\tests\inventories\poc\group_vars", "windows" );

        }
    }
}
