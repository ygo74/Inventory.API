using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Types;
using Inventory.Devices.Api.Configuration;
using Inventory.Devices.Api.Graphql.Queries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.UnitTests.Tests
{
    [TestFixture]
    class CreateServerTests : BaseDbInventoryTests
    {

        [Test]
        public void AddNewServer()
        {
            Assert.Fail();
        }

        [Test]
        public async Task Graphql_schema_is_valid()
        {
            var env = new TestWebEnvironment();
            ISchema schema = await new ServiceCollection()
                .AddGraphQL()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<ServerQueries>()

                .BindRuntimeType<DateTime, DateTimeType>()
                .BindRuntimeType<int, IntType>()
                .BindRuntimeType<long, LongType>()
                .BuildSchemaAsync();

            // assert
            Assert.NotNull(schema.Print());

        }

        [Test]
        public async Task Query_server_is_valid()
        {
            // Arrange
            IRequestExecutor executor = await new ServiceCollection()
                .AddGraphQL()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<ServerQueries>()

                .BindRuntimeType<DateTime, DateTimeType>()
                .BindRuntimeType<int, IntType>()
                .BindRuntimeType<long, LongType>()
                .BuildRequestExecutorAsync();

            // Act
            IExecutionResult result = await executor.ExecuteAsync(@"
                query TestStatus {
                    status
                }
            ");

            // assert
            Assert.NotNull(result.ToJson());

        }


    }

    public class TestWebEnvironment : IWebHostEnvironment
    {
        public string WebRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider WebRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IFileProvider ContentRootFileProvider { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ContentRootPath { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EnvironmentName { 
            get => "Development"; 
            set => throw new NotImplementedException(); 
        }
    }
}
