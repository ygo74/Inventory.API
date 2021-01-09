using Inventory.Domain.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases
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
                    var environments = GetEnvironments();
                    var servers = GetFakeServers(operatingSystems, environments);
                    var groups = GetDefaultGroups();
                    var locations = GetLocations();
                    var location = locations.FirstOrDefault();
                    var applications = GetFakeApplications();
                    var application = applications.FirstOrDefault();

                    foreach (Server srv in servers)
                    {
                        var group = groups.Single(grp => grp.Name == srv.OperatingSystem.Name);
                        group.AddServer(srv);

                        location.AddServer(srv);

                        application.AddServer(srv);
                    }

                    context.Applications.AddRange(applications);
                    context.Locations.AddRange(locations);
                    context.Environments.AddRange(environments);
                    context.OperatingSystems.AddRange(operatingSystems);
                    context.TrustLevels.AddRange(GetTrustLevels());

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


        private List<Server> GetFakeServers(List<Domain.Models.OperatingSystem> operatingSystems, List<Domain.Models.Environment> environments)
        {

            var windows2019 = operatingSystems.Single(os => os.Name.Equals("Windows 2019", StringComparison.OrdinalIgnoreCase));
            var windows2016 = operatingSystems.Single(os => os.Name.Equals("Windows 2016", StringComparison.OrdinalIgnoreCase));
            var rhel7 = operatingSystems.Single(os => os.Name.Equals("RHEL 7", StringComparison.OrdinalIgnoreCase));
            var rhel8 = operatingSystems.Single(os => os.Name.Equals("RHEL 8", StringComparison.OrdinalIgnoreCase));

            var prd = environments.Single(env => env.Name == "prd");
            var sit = environments.Single(env => env.Name == "sit");
            var uat = environments.Single(env => env.Name == "uat");
            var poc = environments.Single(env => env.Name == "poc");


            return new List<Server>()
            {
                new Server("msTest1", windows2019, poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
                new Server("msTest2", windows2019, uat, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
                new Server("msTest3", windows2016, poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
                new Server("msTest4", windows2016, sit, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
                new Server("lxTest1", rhel7,       prd, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
                new Server("lxTest2", rhel7,       poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
                new Server("lxTest3", rhel8,       sit, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
                new Server("lxTest4", rhel8,       poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            };
        }


        private List<Application> GetFakeApplications()
        {
            return new List<Application>()
            {
                new Application("MyApp1", "ap1"),
                new Application("MyApp2", "ap2")
            };
        }

        private List<Group> GetDefaultGroups()
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

        private List<Domain.Models.Environment> GetEnvironments()
        {

            return new List<Domain.Models.Environment>()
            {
                new Domain.Models.Environment("prd", EnvironmentFamilly.Production),
                new Domain.Models.Environment("drp", EnvironmentFamilly.Production),
                new Domain.Models.Environment("dev", EnvironmentFamilly.Developoment),
                new Domain.Models.Environment("sit", EnvironmentFamilly.Tests),
                new Domain.Models.Environment("uat", EnvironmentFamilly.Tests),
                new Domain.Models.Environment("poc", EnvironmentFamilly.Developoment)
            };               
        }

        private List<Location> GetLocations()
        {

            return new List<Location>()
            {
                new Location("Paris", "fr", "par"),
                new Location("Geneva", "ch", "gva")
            };
        }

        private List<TrustLevel> GetTrustLevels()
        {

            return new List<TrustLevel>()
            {
                new TrustLevel("Confidential", "COM"),
                new TrustLevel("Secure", "SEC"),
                new TrustLevel("Administration", "ADM"),
                new TrustLevel("Internet", "INT"),
                new TrustLevel("Extranet", "EXT")
            };
        }

    }
}
