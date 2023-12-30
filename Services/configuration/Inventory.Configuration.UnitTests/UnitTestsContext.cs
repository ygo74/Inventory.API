using Inventory.Configuration.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Inventory.Common.Domain.Repository;
using Inventory.Common.UnitTests;
using Inventory.Common.Infrastructure.Telemetry;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Inventory.Common.Infrastructure.Events;
using Inventory.Common.UnitTests.Events;
using Inventory.Common.Application.Plugins;
using Microsoft.AspNetCore.Http;
using Moq;
using Inventory.Common.Domain.Interfaces;
using Inventory.Configuration.Api.Application.Datacenters.Services;
using Inventory.Configuration.Api.Application.Plugins.Services;
using Inventory.Configuration.Api.Application.Locations.Services;
using Inventory.Configuration.Api.Application.Credentials.Services;
using Inventory.Common.Domain.Models;

namespace Inventory.Configuration.UnitTests
{
    public class UnitTestsContext : TestExecutionContext<Inventory.Configuration.Api.Startup>
    {

        private static readonly UnitTestsContext _context = new UnitTestsContext();
        protected UnitTestsContext() : base()
        {

        }


        public override void Configure(ServiceCollection services, IWebHostEnvironment Environment)
        {
            base.Configure(services, Environment);

            // RabbitMQ
            services.AddSingleton<IEventBus, WithoutEventBus>();

            // Telemetry
            string sourceName = null;
            services.AddTelemetryService(Configuration, out sourceName);

            // Database
            services.AddEntityFrameworkInMemoryDatabase().AddDbContext<ConfigurationDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("in-memory").UseInternalServiceProvider(sp);
            });
            services.AddEntityFrameworkInMemoryDatabase().AddPooledDbContextFactory<ConfigurationDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("in-memory").UseInternalServiceProvider(sp);
                options.EnableDetailedErrors(true);
                options.EnableSensitiveDataLogging(true);

            });
            services.AddScoped(typeof(IAsyncRepositoryWithSpecification<>), typeof(ConfigurationRepositoryWithSpec<>));
            services.AddScoped(typeof(IGenericQueryStore<>), typeof(ConfigurationQueryStore<>));
            services.AddScoped(typeof(IAsyncRepository<>), typeof(ConfigurationRepository<>));


            //pagination
            var mockHttpContext = new Mock<HttpContext>();
            services.AddSingleton<IHttpContextAccessor>(new HttpContextAccessor { HttpContext = mockHttpContext.Object });
            services.AddHttpContextAccessor();
            services.AddPagination();

            // Applications
            services.AddSingleton<PluginResolver>();
            services.AddScoped<PluginService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICredentialService, CredentialService>();
            services.AddScoped<IDatacenterService, DatacenterService>();


            // Unit tests specific configuration
            services.AddScoped<ICurrentUser, CurrentTestUserService>();

        }

        public static UnitTestsContext Current
        {
            get
            {
                return _context;
            }
        }

        public IMediator GetMediator() => GetService<IMediator>();
        public ConfigurationDbContext DbContext => GetService<ConfigurationDbContext>();
        public IAsyncRepository<T> GetAsyncRepository<T>() where T : Entity => GetService<IAsyncRepository<T>>();

    }
}
