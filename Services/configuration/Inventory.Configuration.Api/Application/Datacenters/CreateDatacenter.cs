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
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Validators;

namespace Inventory.Configuration.Api.Application.Datacenters
{
    public class CreateDatacenterRequest : CreateConfigurationEntityRequest<DatacenterDto>
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class CreateDatacenterValidator : CreateConfigurationEntityDtoValidator<CreateDatacenterRequest>
    {
        public CreateDatacenterValidator()
        {

            RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("{PropertyName} is mandatory")
                .NotEmpty().WithMessage("{PropertyName} is mandatory");

            RuleFor(e => e.Code).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

        }
    }

    public class CreateDatacenterHandler : IRequestHandler<CreateDatacenterRequest, Payload<DatacenterDto>>
    {

        private readonly IAsyncRepository<Domain.Models.Datacenter> _repository;
        private readonly ILogger<CreateDatacenterHandler> _logger;
        private readonly IMapper _mapper;

        public CreateDatacenterHandler(IAsyncRepository<Domain.Models.Datacenter> repository, ILogger<CreateDatacenterHandler> logger, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Payload<DatacenterDto>> Handle(CreateDatacenterRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start adding datacenter '{request.Name}' with code '{request.Code}'");
            bool success = false;
            try
            {

                // Map request to Domain entity
                //var newEntity = _mapper.Map<Domain.Models.Datacenter>(request);
                var newEntity = new Domain.Models.Datacenter(request.Code, request.Name, Domain.Models.DatacenterType.Cloud, request.InventoryCode,
                                                         startDate: request.ValidFrom, endDate: request.ValidTo);

                // Add entity
                var result = await _repository.AddAsync(newEntity, cancellationToken);

                // Map response
                success = true;
                return Payload<DatacenterDto>.Success(_mapper.Map<DatacenterDto>(result));
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully adding datacenter '{request.Name}' with code '{request.Code}'");
                else
                    _logger.LogError($"Error when adding datacenter '{request.Name}' with code '{request.Code}'");

            }
        }
    }

}
