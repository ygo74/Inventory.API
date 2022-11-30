using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Behaviors;
using Inventory.Common.Infrastructure.Logging;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.UnitTests
{
    public class TestExecutionContext<AssemblyType> : IDisposable, IAsyncDisposable
    {

        private readonly ServiceProvider _serviceProvider;

        private readonly ServiceCollection _serviceCollection;



        #region IDisposable, IAsyncDisposable
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


        #endregion


        #region Customization
        public virtual void AddCustomService(ServiceCollection services, IWebHostEnvironment Environment)
        {
        }

        public virtual void Configure(ServiceCollection services, IWebHostEnvironment Environment)
        {

        }



        public virtual IConfigurationBuilder AddConfiguration(IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder;
        }

        #endregion

        public TestExecutionContext()
        {
            var loggerConfiguration = LoggingConfiguration.CreateSerilogLoggerConfiguration(Configuration, "UnitTest", "xxx");
            Log.Logger = loggerConfiguration.CreateLogger();


            _serviceCollection = new ServiceCollection();

            // Configuration
            _serviceCollection.AddTransient<IConfiguration>(sp =>
            {
                return Configuration;
            });

            // Environment
            var mockEnvironment = new Mock<IWebHostEnvironment>();
            mockEnvironment.Setup(m => m.EnvironmentName).Returns(Environments.Development);

            _serviceCollection.AddMemoryCache();
            _serviceCollection.AddLogging();

            // MediatR
            _serviceCollection.AddMediatR(typeof(AssemblyType));
            _serviceCollection.AddScoped<IMediator, Mediator>();
            _serviceCollection.AddScoped<ServiceFactory>(p => p.GetService);
            _serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // FluentValidation
            _serviceCollection.AddValidatorsFromAssembly(typeof(AssemblyType).Assembly);

            // AutoMapper
            _serviceCollection.AddAutoMapper(typeof(AssemblyType));

            this.Configure(_serviceCollection, mockEnvironment.Object);
            this.AddCustomService(_serviceCollection, mockEnvironment.Object);

            _serviceProvider = _serviceCollection.BuildServiceProvider();

        }

        public T GetService<T>() => _serviceProvider.GetService<T>();

        public ILogger<T> GetLogger<T>() => _serviceProvider.GetService<ILogger<T>>();

        public IMapper GetMapper() => _serviceProvider.GetService<IMapper>();

        public IConfiguration Configuration => GetConfiguration();

        private IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            this.AddConfiguration(builder);

            return builder.Build();
        }


    }
}
