﻿using Inventory.Configuration.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Common.Domain.Repository;
using Inventory.Common.Infrastructure.Database;
using Inventory.Common.Application.Behaviors;
using FluentValidation;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Inventory.Configuration.Domain.Models;
using Inventory.Common.Application.Users;
using Inventory.Common.UnitTests;
using Inventory.Common.Infrastructure.Telemetry;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Inventory.Common.Infrastructure.Events.RabbitMQ;
using Inventory.Common.Infrastructure.Events;
using Inventory.Common.UnitTests.Events;
using Inventory.Common.Application.Plugins;
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Common.Infrastructure.Logging;

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
            services.AddScoped(typeof(IAsyncRepository<>), typeof(ConfigurationRepository<>));
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



// Applications
services.AddSingleton<PluginResolver>();
            services.AddScoped<PluginService>();


            // Unit tests specific configuration
            services.AddScoped<ICurrentUser, CurrentTestUserService>();

            services.AddScoped<Microsoft.AspNetCore.Http.IHttpContextAccessor>(factory =>
            {
                return new Microsoft.AspNetCore.Http.HttpContextAccessor();
            });

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
        public IAsyncRepository<T> GetAsyncRepository<T>() where T : class => GetService<IAsyncRepository<T>>();

    }
}