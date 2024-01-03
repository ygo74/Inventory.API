using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Google.Protobuf.WellKnownTypes;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Credentials.Dtos;
using Inventory.Configuration.Api.Application.Credentials.Services;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Api.Application.Locations.Services;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Credentials
{
    /// <summary>
    /// Create credential request
    /// </summary>
    public class CreateCredentialRequest : IRequest<Payload<CredentialDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public JsonElement PropertyBag { get; set; }
    }

    /// <summary>
    /// Create credential request validator
    /// </summary>
    public class CreateCredentialRequestValidator : AbstractValidator<CreateCredentialRequest>
    {
        public CreateCredentialRequestValidator(ICredentialService service)
        {
            RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("{PropertyName} is mandatory")
                .MustAsync(async (credentialName, cancellation) =>
                {
                    return !await service.CredentialExists(name: credentialName,
                                                           cancellationToken: cancellation);

                }).WithMessage("Credential's name with value {PropertyValue} already exists in the database");
        }
    }

    /// <summary>
    /// Create credential request handler
    /// </summary>
    public class CreateCredentialHanlder : IRequestHandler<CreateCredentialRequest, Payload<CredentialDto>>
    {
        private readonly ILogger<CreateCredentialHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Credential> _repository;

        public CreateCredentialHanlder(ILogger<CreateCredentialHanlder> logger, IMapper mapper, IAsyncRepository<Credential> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = Guard.Against.Null(repository, nameof(repository));

        }

        public async Task<Payload<CredentialDto>> Handle(CreateCredentialRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start adding Credential '{request.Name}'");

            // Create entity
            var newEntity = new Credential(request.Name, request.Description);

            var inputPropertyBag = JsonSerializer.Deserialize<Dictionary<string, object>>(request.PropertyBag.ToString(), new System.Text.Json.JsonSerializerOptions());
            newEntity.SetPropertyBag(inputPropertyBag);

            // Add new entity
            var result = await _repository.AddAsync(newEntity, cancellationToken);
            if (result <= 0)
            {
                var errorMessage = $"Error when adding Credential '{request.Name}'";
                _logger.LogError(errorMessage);
                return Payload<CredentialDto>.Error(new GenericApiError(errorMessage));
            }

            // return result
            _logger.LogInformation($"Successfully added Credential '{request.Name}'");
            return Payload<CredentialDto>.Success(_mapper.Map<CredentialDto>(newEntity));
        }
    }

}
