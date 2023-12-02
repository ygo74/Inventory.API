using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Inventory.Devices.Infrastructure;
using Inventory.Common.Domain.Repository;
using Inventory.Devices.UnitTests.SeedWork;
using Inventory.Common.Application.Behaviors;
using Inventory.Common.Application.Users;
using Inventory.Common.UnitTests;
using Inventory.Common.Infrastructure.Telemetry;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using HotChocolate;
using HotChocolate.Execution;
using System.Threading.Tasks;
using Inventory.Common.Domain.Interfaces;
using Inventory.Devices.Domain.Interfaces;
using System.Diagnostics;

namespace Inventory.Devices.UnitTests
{
    public class BaseDbInventoryTests : IDisposable, IAsyncDisposable
    {

        private readonly ServiceProvider _serviceProvider;

        private readonly ServiceCollection serviceCollection;

        public virtual void AddCustomService(ServiceCollection services, IWebHostEnvironment Environment)
        {
        }


        public BaseDbInventoryTests()
        {

            serviceCollection = new ServiceCollection();
            // Configuration
            var configuration = GetConfiguration();

            serviceCollection.AddTransient<IConfiguration>(sp =>
            {
                return configuration;
            });

            // Environment
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);

            //Mock IHttpContextAccessor
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            serviceCollection.AddSingleton<IHttpContextAccessor>(sp =>
            {
                return mockHttpContextAccessor.Object;
            });

            serviceCollection.AddMemoryCache();
            serviceCollection.AddLogging(builder =>
            {
                builder.AddDebug().AddConsole();
            });

            // Database
            serviceCollection.AddEntityFrameworkInMemoryDatabase().AddDbContext<ServerDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("in-memory").UseInternalServiceProvider(sp)
                        .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                        .LogTo(message => Debug.WriteLine(message), new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information);
            });

            serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(DevicesRepository<>));
            serviceCollection.AddScoped(typeof(IDeviceQueryStore), typeof(DeviceQueryStore));

            // MediatR
            serviceCollection.AddMediatR(typeof(Inventory.Devices.Api.Startup));
            serviceCollection.AddScoped<IMediator, Mediator>();
            serviceCollection.AddScoped<ServiceFactory>(p => p.GetService);
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // FluentValidation
            serviceCollection.AddValidatorsFromAssembly(typeof(Inventory.Devices.Api.Startup).Assembly);

            // AutoMapper
            serviceCollection.AddAutoMapper(typeof(Inventory.Devices.Api.Startup));

            // Test User
            serviceCollection.AddScoped<ICurrentUser, CurrentTestUserService>();

            // Telemetry
            string sourceName = null;
            serviceCollection.AddTelemetryService(configuration, out sourceName);

            // Extension for custom services
            this.AddCustomService(serviceCollection, mockEnvironment.Object);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Init Database
            InitDatabase();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);
            Dispose(disposing: false);
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
            GC.SuppressFinalize(this);
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceProvider?.Dispose();
            }
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            if (_serviceProvider is not null)
            {
                await _serviceProvider.DisposeAsync().ConfigureAwait(false);
            }
        }

        public ServerDbContext DbContext => _serviceProvider.GetService<ServerDbContext>();

        public T GetService<T>() => _serviceProvider.GetService<T>();

        public ILogger<T> GetLogger<T>() => _serviceProvider.GetService<ILogger<T>>();

        public IMapper GetMapper() => _serviceProvider.GetService<IMapper>();

        public IMediator GetMediator() => _serviceProvider.GetService<IMediator>();

        public IAsyncRepository<T> GetAsyncRepository<T>() where T : class => _serviceProvider.GetService<IAsyncRepository<T>>();


        public async Task<IRequestExecutor> GetExecutor() => await serviceCollection.AddGraphQL().BuildRequestExecutorAsync();

        private void InitDatabase()
        {
            var dbContext = DbContext;

            // Configuration
            dbContext.OperatingSystems.AddRange(OperatingSystemSeed.Get());

            // Credentials

            // servers
            dbContext.Servers.AddRange(ServerSeed.Get());

            dbContext.SaveChanges();

        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

    }

}