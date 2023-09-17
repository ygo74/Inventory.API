using Inventory.Common.Domain.Interfaces;
using Inventory.Common.Infrastructure.Events;
using Inventory.Common.Infrastructure.Telemetry;
using Inventory.Common.UnitTests;
using Inventory.Common.UnitTests.Events;
using Inventory.Provisioning.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Provisioning.UnitTests
{
    public class UnitTestsContext : TestExecutionContext<Inventory.Provisioning.Api.Applications.LabelNames.GetLabelNameById>
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
            services.AddEntityFrameworkInMemoryDatabase().AddDbContext<ProvisioningDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("in-memory").UseInternalServiceProvider(sp);
            });
            services.AddEntityFrameworkInMemoryDatabase().AddPooledDbContextFactory<ProvisioningDbContext>((sp, options) =>
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
        public ProvisioningDbContext DbContext => GetService<ProvisioningDbContext>();
    }
}
