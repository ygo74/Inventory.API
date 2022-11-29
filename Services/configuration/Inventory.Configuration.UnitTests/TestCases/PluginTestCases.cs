using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.UnitTests.SeedWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.TestCases
{
    public static class PluginTestCases
    {
        public static IEnumerable GetCreatePluginsWithCorrectMandatoryValues()
        {
            yield return new CreatePlugin.Command() { Name = "", Code = "TEST-XX" };
            yield return new CreatePlugin.Command() { Name = "TEST-XX", Code = "TEST-XX" };
        }

        public static IEnumerable GetPluginsByCode()
        {
            yield return new GetPluginRequest() { Code = PluginSeed.AZURE_INVENTORY };
            yield return new GetPluginRequest() { Code = PluginSeed.EFFICIENTIP_INVENTORY };
        }

    }
}
