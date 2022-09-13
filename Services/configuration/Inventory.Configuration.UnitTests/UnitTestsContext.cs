using Inventory.Configuration.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Base.Repository;
using Inventory.Infrastructure.Base.Database;
using Inventory.Api.Base.Behaviors;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Inventory.Configuration.Domain.Models;
using Inventory.Api.Base.Users;
using Inventory.UnitTests.Base;
using Inventory.Infrastructure.Base.Telemetry;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Inventory.Configuration.UnitTests
{
    public class UnitTestsContext
    {

        private static readonly UnitTestsContext _context = new UnitTestsContext();
        private readonly ServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        private UnitTestsContext()
        {
            _configuration = GetConfiguration();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddMemoryCache();
            serviceCollection.AddLogging();

            serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(ConfigurationRepository<>));

            // MediatR
            serviceCollection.AddMediatR(typeof(Inventory.Configuration.Api.Startup));
            serviceCollection.AddScoped<IMediator, Mediator>();
            serviceCollection.AddScoped<ServiceFactory>(p => p.GetService);
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // FluentValidation
            serviceCollection.AddValidatorsFromAssembly(typeof(Inventory.Configuration.Api.Startup).Assembly);

            // AutoMapper
            serviceCollection.AddAutoMapper(typeof(Inventory.Configuration.Api.Startup));

            // Telemetry
            string sourceName = null;
            serviceCollection.AddTelemetryService(_configuration, out sourceName);


            // Unit tests specific configuration
            serviceCollection.AddScoped<ICurrentUser, CurrentTestUserService>();
            serviceCollection.AddEntityFrameworkInMemoryDatabase().AddDbContext<ConfigurationDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("in-memory").UseInternalServiceProvider(sp);
            });

            serviceCollection.AddScoped<Microsoft.AspNetCore.Http.IHttpContextAccessor>(factory =>
            {
                return new Microsoft.AspNetCore.Http.HttpContextAccessor();
            });

            _serviceProvider = serviceCollection.BuildServiceProvider();

        }

        public static UnitTestsContext Current
        {
            get
            {
                return _context;
            }
        }

        public ConfigurationDbContext DbContext => _serviceProvider.GetService<ConfigurationDbContext>();

        public T GetService<T>() => _serviceProvider.GetService<T>();

        public ILogger<T> GetLogger<T>() => _serviceProvider.GetService<ILogger<T>>();

        public IMapper GetMapper() => _serviceProvider.GetService<IMapper>();

        public IMediator GetMediator() => _serviceProvider.GetService<IMediator>();

        public IAsyncRepository<T> GetAsyncRepository<T>() where T : class => _serviceProvider.GetService<IAsyncRepository<T>>();

        private IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

    }
}
