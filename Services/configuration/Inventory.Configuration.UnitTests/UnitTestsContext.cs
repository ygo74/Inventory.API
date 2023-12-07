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
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Api.Application.Credentials;
using Microsoft.AspNetCore.Http;
using Moq;
using Inventory.Common.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            services.AddScoped(typeof(IAsyncRepositoryWithSpecification<>), typeof(ConfigurationRepositoryWithSpec<>));
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


            //pagination
            var mockHttpContext = new Mock<HttpContext>();
            services.AddSingleton<IHttpContextAccessor>(new HttpContextAccessor { HttpContext = mockHttpContext.Object });
            services.AddHttpContextAccessor();
            services.AddPagination();

            // Applications
            services.AddSingleton<PluginResolver>();
            services.AddScoped<PluginService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<CredentialService>();


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
        public IAsyncRepositoryWithSpecification<T> GetAsyncRepository<T>() where T : class => GetService<IAsyncRepositoryWithSpecification<T>>();

    }
}
