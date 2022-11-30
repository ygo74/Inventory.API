using AutoMapper;
using FluentValidation;
using HotChocolate;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Exceptions;
using Inventory.Configuration.Domain.Events;
using Inventory.Common.Domain.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenter
{
    public class CreateDatacenter
    {
        [GraphQLName("CreateDatacenterInput")]
        public class Command : CreateConfigurationEntity<Payload>
        {
            public string Name { get; set; }
            public string Code { get; set; }
        }

        public class Payload : BasePayload<Payload, ValidationError>
        {
            public DatacenterDto Datacenter { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {

                RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                    .NotNull().WithMessage("{PropertyName} is mandatory")
                    .NotEmpty().WithMessage("{PropertyName} is mandatory");

                RuleFor(e => e.Code).Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("{PropertyName} is mandatory");

                When(e => e.ValidTo.HasValue && e.ValidFrom.HasValue, () =>
                {
                    RuleFor(e => e.ValidTo).Must((model, validTo) =>
                    {
                        return validTo.Value.CompareTo(model.ValidFrom.Value) > 0;
                    }).WithMessage("{PropertyName} with {PropertyValue} must be greather than ValidFrom date");
                });

            }
        }

        public class Handler : IRequestHandler<Command, Payload>
        {

            private readonly IAsyncRepository<Domain.Models.Datacenter> _repository;
            private readonly ILogger<Handler> _logger;
            private readonly IMapper _mapper;

            public Handler(IAsyncRepository<Domain.Models.Datacenter> repository, ILogger<Handler> logger, IMapper mapper)
            {
                _repository = repository ?? throw new ArgumentNullException(nameof(repository));
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }

            public async Task<Payload> Handle(Command request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Start adding datacenter '{request.Name}' with code '{request.Code}'");

                // Map request to Domain entity
                //var newEntity = _mapper.Map<Domain.Models.Datacenter>(request);
                var newEntity = new Domain.Models.Datacenter(request.Code, request.Name, Domain.Models.DatacenterType.Cloud, request.ValidFrom, request.ValidTo);

                // Add entity
                var result = await _repository.AddAsync(newEntity, cancellationToken);

                // Map response
                _logger.LogInformation($"End adding datacenter '{request.Name}' with code '{request.Code}'");
                return new Payload
                {
                    Datacenter = _mapper.Map<DatacenterDto>(result)
                };
            }
        }

    }
}
