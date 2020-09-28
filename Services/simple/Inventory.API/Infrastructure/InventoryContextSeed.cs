using Inventory.API.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure
{
    public class InventoryContextSeed
    {

        public async Task SeedAsync(InventoryContext context, ILogger<InventoryContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(InventoryContextSeed));

            await policy.ExecuteAsync(async () =>
            {
                if (!context.Groups.Any())
                {
                    var servers = GetFakeServers();
                    var groups = GetFakeGroups();

                    foreach (Server srv in servers)
                    {
                        srv.ServerGroups = new List<ServerGroup>()
                        {
                            new ServerGroup()
                            {
                                Server = srv,
                                Group  = groups.Where(g => g.Name == "operating_system").FirstOrDefault()
                            },
                            new ServerGroup()
                            {
                                Server = srv,
                                Group  = groups.Where(g => g.Name == srv.OperatingSystem.ToString().ToLower()).FirstOrDefault()
                            },
                            new ServerGroup()
                            {
                                Server = srv,
                                Group  = groups.Where(g => g.Name == "location").FirstOrDefault()
                            },
                            new ServerGroup()
                            {
                                Server = srv,
                                Group  = groups.Where(g => g.Name == "france").FirstOrDefault()
                            },
                        };

                    }

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


        private List<Server> GetFakeServers()
        {
            return new List<Server>()
            {
                new Server()
                {
                    ServerId = 1,
                    Name   = "msTest1",
                    OperatingSystem = OsType.Windows
                },
                new Server()
                {
                    ServerId = 2,
                    Name   = "msTest2",
                    OperatingSystem = OsType.Windows
                },
                new Server()
                {
                    ServerId = 3,
                    Name   = "lxTest3",
                    OperatingSystem = OsType.Linux
                },
                new Server()
                {
                    ServerId = 4,
                    Name   = "lxTest4",
                    OperatingSystem = OsType.Linux
                }
            };
        }

        private List<Group> GetFakeGroups()
        {
            return new List<Group>()
            {
                new Group() {
                    GroupId = 1,
                    Name = "operating_system"
                },
                new Group() {
                    GroupId = 2,
                    Name = "windows"
                },
                new Group() {
                    GroupId = 3,
                    Name = "linux"
                },
                new Group() {
                    GroupId = 4,
                    Name = "location"
                },
                new Group() {
                    GroupId = 5,
                    Name = "france"
                },
                new Group() {
                    GroupId = 6,
                    Name = "paris"
                }
            };
        }


    }
}
