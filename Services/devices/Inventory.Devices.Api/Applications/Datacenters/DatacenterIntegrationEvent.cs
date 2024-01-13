using Ardalis.GuardClauses;
using Inventory.Common.Infrastructure.Events;
using Inventory.Devices.Domain.Models;
using Inventory.Devices.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.Datacenters
{
    public class DatacenterIntegrationEvent : IntegrationEvent
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string InventoryCode { get; set; }
        public bool Deprecated { get; set; }
        public bool IsValid { get; set; }

    }

    public class DatacenterIntegrationEventHandler : IIntegrationEventHandler<DatacenterIntegrationEvent>
    {
        private readonly ILogger<DatacenterIntegrationEventHandler> _logger;
        private readonly IDbContextFactory<ServerDbContext> _dbContextFactory;

        public DatacenterIntegrationEventHandler(ILogger<DatacenterIntegrationEventHandler> logger, IDbContextFactory<ServerDbContext> dbContextFactory)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _dbContextFactory = Guard.Against.Null(dbContextFactory, nameof(dbContextFactory));
        }

        public Task Handle(DatacenterIntegrationEvent @event)
        {
            _logger.LogInformation("Handling integration event: {0} at {1}.", @event.Id, @event.CreationDate);

            var dataCenter = new DataCenter(@event.Name);

            using var _dbContext = _dbContextFactory.CreateDbContext();
            _dbContext.Datacenters.Add(dataCenter);
            var result = _dbContext.SaveChanges();

            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }

}
