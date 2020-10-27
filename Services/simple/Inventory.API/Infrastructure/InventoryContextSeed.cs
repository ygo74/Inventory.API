using Inventory.Domain.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure
{
    public class InventoryContextSeed
    {

        public async Task SeedAsync(InventoryDbContext context, ILogger<InventoryContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(InventoryContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Groups.Any())
                {
                    var operatingSystems = GetOperatingSystems();
                    var servers = GetFakeServers(operatingSystems);
                    var groups = GetFakeGroups();

                    var sv = new StringVariable() { Name = "a", Value = "a" };
                    var nv = new NumericVariable() { Name = "b", Value = 1 };
                    var lv = new List<Variable>() { sv, nv };

                    groups[0].Variables = lv;

                    foreach (Server srv in servers)
                    {

                        //srv.Variables = lv;

                        var group = groups.Single(grp => grp.Name == srv.OperatingSystem.Name);
                        group.AddServer(srv);
                    }

                    context.OperatingSystems.AddRange(operatingSystems);
                    context.Groups.AddRange(groups);
                    context.Servers.AddRange(servers);

                    await context.SaveChangesAsync();

                }
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<InventoryContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<Npgsql.NpgsqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
                    }
                );
        }


        private List<Server> GetFakeServers(List<Domain.Models.OperatingSystem> operatingSystems)
        {

            var windows2019 = operatingSystems.Single(os => os.Name.Equals("Windows 2019", StringComparison.OrdinalIgnoreCase));
            var windows2016 = operatingSystems.Single(os => os.Name.Equals("Windows 2016", StringComparison.OrdinalIgnoreCase));
            var rhel7 = operatingSystems.Single(os => os.Name.Equals("RHEL 7", StringComparison.OrdinalIgnoreCase));
            var rhel8 = operatingSystems.Single(os => os.Name.Equals("RHEL 8", StringComparison.OrdinalIgnoreCase));

            return new List<Server>()
            {
                new Server("msTest1", windows2019),
                new Server("msTest2", windows2019),
                new Server("msTest3", windows2016),
                new Server("msTest4", windows2016),
                new Server("lxTest1", rhel7),
                new Server("lxTest2", rhel7),
                new Server("lxTest3", rhel8),
                new Server("lxTest4", rhel8)
            };
        }

        private List<Group> GetFakeGroups()
        {

            //OS Groups
            var osGroups = new Group(name: "OperatingSystems", ansibleGroupName: "os");

            var windowsGroups = new Group(name: "Windows");
            var linuxGroups = new Group(name: "Linux");

            osGroups.AddSubGroups(windowsGroups);
            osGroups.AddSubGroups(linuxGroups);

            windowsGroups.AddSubGroups(new Group("Windows 2019", "system_windows_2k19"));
            windowsGroups.AddSubGroups(new Group("Windows 2016", "system_windows_2k16"));

            linuxGroups.AddSubGroups(new Group("RHEL 7", "system_rhel_7"));
            linuxGroups.AddSubGroups(new Group("RHEL 8", "system_rhel_8"));

            var allgroups = new List<Group>()
            {
                osGroups,
                windowsGroups,
                linuxGroups
            };
            allgroups.AddRange(windowsGroups.Children);
            allgroups.AddRange(linuxGroups.Children);

            return allgroups;

        }

        private List<Domain.Models.OperatingSystem> GetOperatingSystems()
        {
            return new List<Domain.Models.OperatingSystem>()
            {
                new Domain.Models.OperatingSystem("Windows 2019", OsFamilly.Windows),
                new Domain.Models.OperatingSystem("Windows 2016", OsFamilly.Windows),
                new Domain.Models.OperatingSystem("RHEL 7", OsFamilly.Linux),
                new Domain.Models.OperatingSystem("RHEL 8", OsFamilly.Linux)
            };
        }


    }
}
