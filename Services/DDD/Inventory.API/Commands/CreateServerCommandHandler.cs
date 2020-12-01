using Inventory.API.Dto;
using Inventory.API.Infrastructure;
using Inventory.Domain;
using Inventory.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Inventory.API.Commands
{
    public class CreateServerCommandHandler : IRequestHandler<CreateServerCommand, ServerDto>
    {

        private readonly InventoryService _inventoryService;
        private readonly ILogger<CreateServerCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly GraphQLService _graphQLService;

        public CreateServerCommandHandler(InventoryService inventoryService, GraphQLService graphQLService, ILogger<CreateServerCommandHandler> logger, IMediator mediator)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _graphQLService = graphQLService ?? throw new ArgumentNullException(nameof(graphQLService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        public async Task<ServerDto> Handle(CreateServerCommand request, CancellationToken cancellationToken)
        {
            var subnetIp = System.Net.IPAddress.Parse(request.SubnetIp);
            var server = await _inventoryService.AddServerAsync(request.HostName, request.OsFamilly, request.Os, request.Environment, subnetIp);

            return await _graphQLService.GetOrFillServerData(server);
        }
    }
}
