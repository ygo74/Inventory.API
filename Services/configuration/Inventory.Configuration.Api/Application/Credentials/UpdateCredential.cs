using Ardalis.GuardClauses;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Credentials.Dtos;
using Inventory.Configuration.Api.Application.Credentials.Services;
using Inventory.Configuration.Api.Application.Credentials.Validators;
using Inventory.Configuration.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Credentials
{
    /// <summary>
    /// Update credential request
    /// </summary>
    public class UpdateCredentialRequest : IRequest<Payload<CredentialDto>>, ICredentialId
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public JsonElement PropertyBag { get; set; }

    }

    /// <summary>
    /// Update credential request validator
    /// </summary>
    public class UpdateCredentialRequestValidator : AbstractValidator<UpdateCredentialRequest>
    {
        public UpdateCredentialRequestValidator(ICredentialService service)
        {
            Include(new CredentialExistByIdValidator(service));
        }
    }

    /// <summary>
    /// Update credential request handler
    /// </summary>
    public class UpdateCredentialHanlder : IRequestHandler<UpdateCredentialRequest, Payload<CredentialDto>>
    {
        private readonly ILogger<UpdateCredentialHanlder> _logger;
        private readonly IAsyncRepository<Credential> _repository;


        public UpdateCredentialHanlder(ILogger<UpdateCredentialHanlder> logger, IAsyncRepository<Credential> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = Guard.Against.Null(repository, nameof(repository));
        }

        public async Task<Payload<CredentialDto>> Handle(UpdateCredentialRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating Credential with id '{request.Id}'");

            // Find entity
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (null == entity)
                return Payload<CredentialDto>.Error(new NotFoundError($"Don't find Credential with Id {request.Id}"));

            // Update Properties
            if (request.Description != null)
                entity.SetDescription(request.Description);

            if (request.PropertyBag.ValueKind != JsonValueKind.Null && request.PropertyBag.ValueKind != JsonValueKind.Undefined)
            {
                var inputPropertyBag = JsonSerializer.Deserialize<Dictionary<string, object>>(request.PropertyBag.ToString(), new System.Text.Json.JsonSerializerOptions());
                entity.SetPropertyBag(inputPropertyBag);
            }

            // Update entity
            var nbChanges = await _repository.UpdateAsync(entity, cancellationToken);
            if (nbChanges > 0)
                _logger.LogInformation($"Successfully updated Credential with id '{request.Id}'");

            // return response
            return Payload<CredentialDto>.Success(entity.ToCredentialDto());

        }
    }

}
