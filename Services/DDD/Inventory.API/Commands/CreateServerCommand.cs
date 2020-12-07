using FluentResults;
using FluentValidation;
using Inventory.API.Dto;
using Inventory.API.Infrastructure;
using Inventory.Domain;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Commands
{
    public class CreateServerCommand : IRequest<ServerDto>
    {
        public String HostName { get; set; }
        public OsFamilly OsFamilly { get; set; }
        public String Os { get; set; }
        public String Environment { get; set; }
        public String SubnetIp { get; set; }
        public IEnumerable<DiskDto> Disks { get; set; }


        //internal sealed class CreateServerCommandHandler : IRequestHandler<CreateServerCommand, ServerDto>
        //{

        //    private readonly InventoryService _inventoryService;
        //    private readonly ILogger<CreateServerCommandHandler> _logger;
        //    private readonly IMediator _mediator;
        //    private readonly GraphQLService _graphQLService;

        //    public CreateServerCommandHandler(InventoryService inventoryService, GraphQLService graphQLService, ILogger<CreateServerCommandHandler> logger, IMediator mediator)
        //    {
        //        _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
        //        _graphQLService = graphQLService ?? throw new ArgumentNullException(nameof(graphQLService));
        //        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        //    }

        //    public async Task<ServerDto> Handle(CreateServerCommand request, CancellationToken cancellationToken)
        //    {

        //        //var validator = new CreateServerValidator();
        //        //var validationResults = validator.Validate(request);

        //        var subnetIp = System.Net.IPAddress.Parse(request.SubnetIp);
        //        var server = await _inventoryService.AddServerAsync(request.HostName, request.OsFamilly, request.Os, request.Environment, subnetIp);

        //        var serverDto = await _graphQLService.GetOrFillServerData(server);
        //        //return Result.Ok(serverDto);
        //        return serverDto;

        //    }
        //}


        //internal sealed class CreateServerValidator : AbstractValidator<CreateServerCommand>
        //{
        //    private readonly IAsyncRepository<Server> _serverRepository;

        //    public CreateServerValidator(IAsyncRepository<Server> serverRepository)
        //    {

        //        _serverRepository = serverRepository;

        //        RuleFor(cs => cs.HostName)
        //            .NotNull().NotEmpty().WithErrorCode("SRV-01")
        //            .MustAsync(async (hostname, cancellation) =>
        //            {
        //                var existingServer = await _serverRepository.FirstOrDefaultAsync(new ServerSpecification(hostname));
        //                return (existingServer == null);
        //            }).WithMessage("'{PropertyName}' Must be unique in the database").WithErrorCode("SRV-02");


        //        RuleFor(cs => cs.SubnetIp).Must(ip =>
        //        {
        //            System.Net.IPAddress iPAddress;
        //            return System.Net.IPAddress.TryParse(ip, out iPAddress);
        //        }).WithMessage("'{PropertyName}' Must be a valid IP").WithErrorCode("SRV-03");
        //    }
        //}

    }
}
