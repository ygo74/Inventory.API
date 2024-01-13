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
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Credentials
{
    /// <summary>
    /// Remove Credential Request
    /// </summary>
    public class RemoveCredentialRequest : IRequest<Payload<CredentialDto>>, ICredentialId
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Remove credential request validator
    /// </summary>
    public class RemoveCredentialRequestValidator : AbstractValidator<RemoveCredentialRequest>
    {
        public RemoveCredentialRequestValidator(ICredentialService service)
        {
            Include(new CredentialExistByIdValidator(service));
        }
    }


    /// <summary>
    /// Remove Credential handler
    /// </summary>
    public class RemoveCredentialHanlder : IRequestHandler<RemoveCredentialRequest, Payload<CredentialDto>>
    {
        private readonly ILogger<RemoveCredentialHanlder> _logger;
        private readonly IAsyncRepository<Credential> _repository;


        public RemoveCredentialHanlder(ILogger<RemoveCredentialHanlder> logger, IAsyncRepository<Credential> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = Guard.Against.Null(repository, nameof(repository));

        }

        public async Task<Payload<CredentialDto>> Handle(RemoveCredentialRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start removing Credential with id '{request.Id}'");

            // Find entity
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (null == entity)
                return Payload<CredentialDto>.Error(new NotFoundError($"Don't find Credential with Id {request.Id}"));

            // Remove entity            
            var nbChanges = await _repository.DeleteAsync(entity, cancellationToken);
            if (nbChanges > 0)
                _logger.LogInformation($"Successfully removed Credential with id '{request.Id}'");

            // return result
            return Payload<CredentialDto>.Success(default(CredentialDto));
        }
    }

}
