using Docker.DotNet;
using Docker.DotNet.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests
{
    public class PostgresqlDocker
    {

        const String CONTAINER_NAME = "InventoryDBTests";
        const String IMAGE_NAME = "postgres";
        const String IMAGE_TAG = "latest";


        public async Task Start()
        {
            using (var conf = new DockerClientConfiguration())
            {
                using (var client = conf.CreateClient())
                {
                    var containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                    var container = containers.FirstOrDefault(c => c.Names.Contains("/" + CONTAINER_NAME));

                    if (container == null)
                    {
                        // Download Images
                        await client.Images.CreateImageAsync(new ImagesCreateParameters()
                        {
                            FromImage = IMAGE_NAME,
                            Tag = IMAGE_TAG
                        },
                                                            new AuthConfig(), new Progress<JSONMessage>());


                        // Create the container
                        var config = new Config()
                        {
                            Hostname = "localhost",
                            //ExposedPorts = new Dictionary<string, EmptyStruct>
                            //{
                            //    {
                            //        "55432", default(EmptyStruct)
                            //    }
                            //},
                            Env = new List<String>()
                            {
                                "POSTGRES_PASSWORD=bloguser",
                                "POSTGRES_USER=bloguser",
                                "POSTGRES_DB=blogdb"
                            }
                        };

                        // Configure the ports to expose
                        var hostConfig = new HostConfig()
                        {
                            PortBindings = new Dictionary<string, IList<PortBinding>>
                            {
                                { "5432/tcp", new List<PortBinding> { new PortBinding { HostIP = "127.0.0.1", HostPort = "55432" } } }
                            },
                            PublishAllPorts = true                            
                        };

                        // Create the container
                        var response = await client.Containers.CreateContainerAsync(new CreateContainerParameters(config)
                        {
                            Image = IMAGE_NAME + ":" + IMAGE_TAG,
                            Name = CONTAINER_NAME,
                            Tty = false,
                            HostConfig = hostConfig
                        });

                        containers = await client.Containers.ListContainersAsync(new ContainersListParameters() { All = true });
                        container = containers.First(c => c.ID == response.ID);
                    }


                    // Start the container is needed
                     if (container.State != "running")
                    {
                        var started = await client.Containers.StartContainerAsync(container.ID, new ContainerStartParameters());
                        if (!started)
                        {
                            Assert.Fail("Cannot start the docker container");
                        }
                    }
                }
            }
        }
    }
}
