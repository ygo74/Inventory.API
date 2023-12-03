using AutoMapper;
using Inventory.Configuration.Domain.Events;
using Inventory.Common.Domain.Models;
using Inventory.Common.Infrastructure.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    public class DatacenterEventHandler : INotificationHandler<Event<Domain.Models.Datacenter>>
    {

        private readonly IEventBus _eventBus;
        private readonly IMapper _mapper;
        private readonly ILogger<DatacenterEventHandler> _logger;


        public DatacenterEventHandler(IEventBus eventBus, ILogger<DatacenterEventHandler> logger, IMapper mapper)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public Task Handle(Event<Domain.Models.Datacenter> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start processing datacenter event '{notification.Data.Name}' with code '{notification.Data.Code}' on action {notification.Action}");

            var dcEvent = _mapper.Map<DatacenterIntegrationEvent>(notification.Data);
            _eventBus.Publish(dcEvent, "dctest");

            _logger.LogInformation($"End processing datacenter event '{notification.Data.Name}' with code '{notification.Data.Code}' on action {notification.Action}");
            return Task.CompletedTask;
        }
    }
}
