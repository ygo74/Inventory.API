using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Inventory.Servers.Infrastructure;
using Inventory.Domain.Base.Repository;
using Inventory.Servers.UnitTests.SeedWork;

namespace Inventory.Servers.UnitTests
{
    public class BaseDbInventoryTests : IDisposable
    {

        private readonly ServiceProvider _serviceProvider;

        public BaseDbInventoryTests()
        {

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddMemoryCache();
            serviceCollection.AddLogging();

            // Database
            serviceCollection.AddEntityFrameworkInMemoryDatabase().AddDbContext<ServerDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("in-memory").UseInternalServiceProvider(sp);
            });

            //serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            // MediatR
            serviceCollection.AddMediatR(typeof(Inventory.Servers.Api.Startup));
            serviceCollection.AddScoped<IMediator, Mediator>();
            serviceCollection.AddScoped<ServiceFactory>(p => p.GetService);
            //serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

            // FluentValidation
            serviceCollection.AddValidatorsFromAssembly(typeof(Inventory.Servers.Api.Startup).Assembly);

            // AutoMapper
            serviceCollection.AddAutoMapper(typeof(Inventory.Servers.Api.Startup));

            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Init Database
            InitDatabase();
        }

        public void Dispose()
        {
            //_dbContext.Database.EnsureDeleted();
            //_dbContext.Dispose();

            if (null != _serviceProvider)
            {
                _serviceProvider.Dispose();
            }
        }

        public ServerDbContext DbContext => _serviceProvider.GetService<ServerDbContext>();

        public T GetService<T>() => _serviceProvider.GetService<T>();

        public ILogger<T> GetLogger<T>() => _serviceProvider.GetService<ILogger<T>>();

        public IMapper GetMapper() => _serviceProvider.GetService<IMapper>();

        public IMediator GetMediator() => _serviceProvider.GetService<IMediator>();

        public IAsyncRepository<T> GetAsyncRepository<T>() where T : class => _serviceProvider.GetService<IAsyncRepository<T>>();

        private void InitDatabase()
        {
            var dbContext = DbContext;

            // Configuration

            // Credentials

            // servers
            dbContext.Servers.AddRange(ServerSeed.Get());

            dbContext.SaveChanges();

        }

    }

}