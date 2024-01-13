using Inventory.Configuration.Api.Application.Plugins;
using Inventory.Configuration.UnitTests.SeedWork;
using System.Collections;

namespace Inventory.Configuration.UnitTests.TestCases
{
    internal static class PluginTestCases
    {
        internal static IEnumerable GetCreatePluginsWithCorrectMandatoryValues()
        {
            yield return new CreatePluginRequest() { Name = "", Code = "TEST-XX" };
            yield return new CreatePluginRequest() { Name = "TEST-XX", Code = "TEST-XX" };
        }

        internal static IEnumerable GetPluginsByCodeAndName()
        {
            yield return new GetPluginRequest() { Code = PluginSeed.AZURE_INVENTORY, Name = PluginSeed.AZURE_INVENTORY };
            yield return new GetPluginRequest() { Code = PluginSeed.EFFICIENTIP_INVENTORY, Name = PluginSeed.EFFICIENTIP_INVENTORY };
        }

    }
}
