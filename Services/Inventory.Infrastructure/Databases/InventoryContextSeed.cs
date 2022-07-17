using Inventory.Domain.Enums;
using Inventory.Domain.Models;
using Inventory.Domain.Models.Configuration;
using Inventory.Domain.Models.ManagedEntities;
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
                if (!context.DataCenters.Any())
                {
                    var datacenters = GetDataCenters();
                    var operatingSystems = GetOperatingSystems();
                    var environments = GetEnvironments();
                    var servers = GetFakeServers(operatingSystems, environments);
                    var locations = GetLocations();
                    var location = locations.FirstOrDefault();
                    var applications = GetFakeApplications();
                    var application = applications.FirstOrDefault();

                    foreach (Server srv in servers)
                    {
                        location.AddServer(srv);

                        application.AddServer(srv);
                    }

                    context.DataCenters.AddRange(datacenters);
                    context.Applications.AddRange(applications);
                    context.Locations.AddRange(locations);
                    context.Environments.AddRange(environments);
                    context.OperatingSystems.AddRange(operatingSystems);
                    context.TrustLevels.AddRange(GetTrustLevels());

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


        private List<Server> GetFakeServers(List<Domain.Models.Configuration.OperatingSystem> operatingSystems, List<Domain.Models.Configuration.Environment> environments)
        {

            //var windows2019 = operatingSystems.Single(os => os.Model.Equals("Windows 2019", StringComparison.OrdinalIgnoreCase));
            //var windows2016 = operatingSystems.Single(os => os.Model.Equals("Windows 2016", StringComparison.OrdinalIgnoreCase));
            //var rhel7 = operatingSystems.Single(os => os.Model.Equals("RHEL 7", StringComparison.OrdinalIgnoreCase));
            //var rhel8 = operatingSystems.Single(os => os.Model.Equals("RHEL 8", StringComparison.OrdinalIgnoreCase));

            //var prd = environments.Single(env => env.Name == "prd");
            //var sit = environments.Single(env => env.Name == "sit");
            //var uat = environments.Single(env => env.Name == "uat");
            //var poc = environments.Single(env => env.Name == "poc");


            //return new List<Server>()
            //{
            //    new Server("msTest1", windows2019, poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //    new Server("msTest2", windows2019, uat, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //    new Server("msTest3", windows2016, poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //    new Server("msTest4", windows2016, sit, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //    new Server("lxTest1", rhel7,       prd, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //    new Server("lxTest2", rhel7,       poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //    new Server("lxTest3", rhel8,       sit, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //    new Server("lxTest4", rhel8,       poc, 2, 4, System.Net.IPAddress.Parse("192.168.1.0")),
            //};

            return new List<Server>();
        }


        private List<Application> GetFakeApplications()
        {
            return new List<Application>()
            {
                new Application("MyApp1", "ap1"),
                new Application("MyApp2", "ap2")
            };
        }


        private List<Domain.Models.Configuration.OperatingSystem> GetOperatingSystems()
        {
            return new List<Domain.Models.Configuration.OperatingSystem>()
            {
            };
        }

        private List<Domain.Models.Configuration.Environment> GetEnvironments()
        {

            return new List<Domain.Models.Configuration.Environment>()
            {
            };               
        }

        private List<Location> GetLocations()
        {

            return new List<Location>()
            {
            };
        }

        private List<TrustLevel> GetTrustLevels()
        {

            return new List<TrustLevel>()
            {
            };
        }

        private List<DataCenter> GetDataCenters()
        {
            return new List<DataCenter>()
            {
                new DataCenter("Azure1", "Azure", DataCenterType.Cloud)
            };
        }
    }


}
