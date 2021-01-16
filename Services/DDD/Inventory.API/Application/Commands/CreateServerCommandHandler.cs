using Inventory.API.Application.Dto;
using Inventory.API.Infrastructure;
using Inventory.Domain;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;


namespace Inventory.API.Application.Commands
{
    public class CreateServerCommandHandler : IRequestHandler<CreateServerCommand, ServerDto>
    {

        private readonly InventoryService _inventoryService;
        private readonly ILogger<CreateServerCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly GraphQLService _graphQLService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateServerCommandHandler(InventoryService inventoryService, GraphQLService graphQLService, 
                                          ILogger<CreateServerCommandHandler> logger, IMediator mediator, 
                                          IHttpContextAccessor httpContextAccessor)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _graphQLService = graphQLService ?? throw new ArgumentNullException(nameof(graphQLService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            

        }

        public async Task<ServerDto> Handle(CreateServerCommand request, CancellationToken cancellationToken)
        {

            var subnetIp = System.Net.IPAddress.Parse(request.SubnetIp);
            var server = await _inventoryService.AddServerAsync(request.HostName, request.OsFamilly, request.Os, request.Environment, subnetIp);

            var serverDto = await _graphQLService.GetOrFillServerData(server);
            //return Result.Ok(serverDto);
            return serverDto;

        }
    }
}
