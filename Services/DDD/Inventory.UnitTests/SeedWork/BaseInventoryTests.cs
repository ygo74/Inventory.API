using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Infrastructure.Databases;
using Inventory.Infrastructure.Databases.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Npgsql.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.UnitTests
{
    public class BaseInventoryTests<T> : IDisposable
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


        private InventoryDbContext _dbContext;
        public InventoryDbContext DbContext
        {
            get { return _dbContext; }
        }


        private ILogger<T> _logger;
        public ILogger<T> Logger
        {
            get { return _logger;  }
        }

        public BaseInventoryTests()
        {

            var loggerFactory = LoggerFactory.Create(builder => builder.AddDebug());

            var _dbOptions = new DbContextOptionsBuilder<InventoryDbContext>()
                                                                                    .UseNpgsql("host=localhost;port=55432;database=blogdb;username=bloguser;password=bloguser",
                                                                                    npgOptions =>
                                                                                    {
                                                                                        npgOptions.EnableRetryOnFailure();
                                                                                    })//.UseSnakeCaseNamingConvention()
                                                                                    .UseLoggerFactory(loggerFactory)
                                                                                    .Options;


        _dbContext = new InventoryDbContext(_dbOptions);
            _logger = new Logger<T>(new NullLoggerFactory());

            if (_dbContext.Database.ProviderName == "Npgsql.EntityFrameworkCore.PostgreSQL")
            {
                PostgresqlDocker docker = new PostgresqlDocker();
                docker.Start().Wait();
            }

            //TODO : How to wait DB is ready to accept connection
            _dbContext.Database.EnsureCreated();
            //_dbContext.Database.Migrate();

            var logger = new Logger<InventoryContextSeed>(new NullLoggerFactory());
            new InventoryContextSeed().SeedAsync(_dbContext, logger).Wait();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }

}