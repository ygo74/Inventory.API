﻿using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Domain.Repository;
using Inventory.Devices.Api.Graphql.Mutations;
using Inventory.Devices.Domain.Models;
using Inventory.Devices.Domain.Specifications;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Applications.Servers
{
    public class CreateServer
    {
        public class Command : IRequest<CreateServerPayload>
        {
            public string Hostname { get; set; }
        }

        public class CreateServerPayload : Payload<ServerDto>
        {
        }

        public class Validator : AbstractValidator<Command>
        {
            private readonly IAsyncRepositoryWithSpecification<Server> _serverRepository;

            public Validator(IAsyncRepositoryWithSpecification<Server> serverRepository)
            {

                _serverRepository = serverRepository;

                RuleFor(cs => cs.Hostname)
                    .NotNull().NotEmpty().WithErrorCode("SRV-01")
                    .MustAsync(async (hostname, cancellation) =>
                    {
                        var s = new ServerSpecification(hostname);
                        var existingServer = await _serverRepository.FirstOrDefaultAsync(new ServerSpecification(hostname));
                        return (existingServer == null);
                    }).WithMessage("'{PropertyName}' Must be unique in the database").WithErrorCode("SRV-02");


                //RuleFor(cs => cs.SubnetIp).Must(ip =>
                //{
                //    System.Net.IPAddress iPAddress;
                //    return System.Net.IPAddress.TryParse(ip, out iPAddress);
                //}).WithMessage("'{PropertyName}' Must be a valid IP").WithErrorCode("SRV-03");
            }
        }

        public class Handler : IRequestHandler<Command, CreateServerPayload>
        {

            private readonly IAsyncRepositoryWithSpecification<Server> _repository;
            private readonly ILogger<Handler> _logger;
            private readonly IMediator _mediator;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly IMapper _mapper;


            public Handler(ILogger<Handler> logger, IMediator mediator, IMapper mapper, IAsyncRepositoryWithSpecification<Server> repository,
                                              IHttpContextAccessor httpContextAccessor)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
                _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            }

            public async Task<CreateServerPayload> Handle(Command request, CancellationToken cancellationToken)
            {
                // Map request to Domain entity
                var newEntity = _mapper.Map<Server>(request);


                // Add entity
                newEntity.SetDataCenter(1);
                newEntity.SetOperatingSystem(1);
                var result = await _repository.AddAsync(newEntity, cancellationToken);

                // Map response
                return new CreateServerPayload
                {
                    Data = _mapper.Map<ServerDto>(result)
                };

            }
        }


    }
}
