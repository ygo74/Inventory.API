using AutoMapper;
using FluentValidation;
using Inventory.API.Commands.Application.Behaviors;
using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Infrastructure.Databases;
using Inventory.Infrastructure.Databases.Repositories;
using Inventory.UnitTests.SeedWork.Configuration;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Npgsql.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.UnitTests
{
    public class BaseDbInventoryTests : IDisposable
    {
        //private readonly DbContextOptions<InventoryDbContext> _dbOptions = new DbContextOptionsBuilder<InventoryDbContext>()
        //                                                                    .UseInMemoryDatabase(databaseName: "in-memory")
        //                                                                    .Options;


        //private static ILoggerFactory loggerFactory = new LoggerFactory();

        //private readonly DbContextOptions<InventoryDbContext> _dbOptions = new DbContextOptionsBuilder<InventoryDbContext>()
        //                                                                            .UseNpgsql("host=localhost;port=55432;database=blogdb;username=bloguser;password=bloguser",
        //                                                                            npgOptions => 
        //                                                                            {
        //                                                                                npgOptions.EnableRetryOnFailure();
        //                                                                            })//.UseSnakeCaseNamingConvention()
        //                                                                            .UseLoggerFactory(loggerFactory)
        //                                                                            .Options;


        private readonly ServiceProvider _serviceProvider;

        //private InventoryDbContext _dbContext;
        //public InventoryDbContext DbContext
        //{
        //    get { return _dbContext; }
        //}


        //private ILogger<T> _logger;
        //public ILogger<T> Logger
        //{
        //    get { return _logger;  }
        //}

        public BaseDbInventoryTests()
        {

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMemoryCache();
            serviceCollection.AddLogging();

            // Database
            serviceCollection.AddEntityFrameworkInMemoryDatabase().AddDbContext<InventoryDbContext>((sp, options) =>
            {
                options.UseInMemoryDatabase("in-memory").UseInternalServiceProvider(sp);
            });

            serviceCollection.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));

            // MediatR
            serviceCollection.AddMediatR(typeof(Inventory.API.Startup));
            serviceCollection.AddScoped<IMediator, Mediator>();
            serviceCollection.AddScoped<ServiceFactory>(p => p.GetService);
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));

            // FluentValidation
            serviceCollection.AddValidatorsFromAssembly(typeof(Inventory.API.Startup).Assembly);

            // AutoMapper
            serviceCollection.AddAutoMapper(typeof(Inventory.API.Startup));


            _serviceProvider = serviceCollection.BuildServiceProvider();

            // Init Database
            InitDatabase();

            //var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());

            //var _dbOptions = new DbContextOptionsBuilder<InventoryDbContext>()
            //                                                                        .UseNpgsql("host=localhost;port=55432;database=blogdb;username=bloguser;password=bloguser",
            //                                                                        npgOptions =>
            //                                                                        {
            //                                                                            npgOptions.EnableRetryOnFailure();
            //                                                                        })//.UseSnakeCaseNamingConvention()
            //                                                                        .UseLoggerFactory(loggerFactory)
            //                                                                        .Options;


            //_dbContext = new InventoryDbContext(_dbOptions);
            //_logger = new Logger<T>(new NullLoggerFactory());

            //if (_dbContext.Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
            //{
            //    PostgresqlDocker docker = new PostgresqlDocker();
            //    docker.Start().Wait();
            //}

            ////TODO : How to wait DB is ready to accept connection
            //_dbContext.Database.EnsureCreated();
            ////_dbContext.Database.Migrate();

            //var logger = new Logger<InventoryContextSeed>(new NullLoggerFactory());
            //new InventoryContextSeed().SeedAsync(_dbContext, logger).Wait();




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

        public InventoryDbContext DbContext => _serviceProvider.GetService<InventoryDbContext>();

        public ILogger<T> GetLogger<T>() => _serviceProvider.GetService<ILogger<T>>();

        public IMediator GetMediator() => _serviceProvider.GetService<IMediator>();

        public IAsyncRepository<T> GetAsyncRepository<T>() where T : class => _serviceProvider.GetService<IAsyncRepository<T>>();

        private void InitDatabase()
        {
            var dbContext = DbContext;

            dbContext.TrustLevels.AddRange(TrustLevelSeed.Get());
            dbContext.DataCenters.AddRange(DataCenterSeed.Get());
            dbContext.Locations.AddRange(LocationSeed.Get());
            dbContext.Environments.AddRange(EnvironmentSeed.Get());
            dbContext.OperatingSystems.AddRange(OperatingSystemSeed.Get());
            dbContext.SaveChanges();

        }

    }

}