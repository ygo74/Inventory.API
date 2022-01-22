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

        private readonly ILogger<CreateServerCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateServerCommandHandler(ILogger<CreateServerCommandHandler> logger, IMediator mediator, 
                                          IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            

        }

        public async Task<ServerDto> Handle(CreateServerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

        }
    }
}
