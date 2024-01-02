using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Errors;
using Inventory.Common.Domain.Repository;
using Inventory.Configuration.Api.Application.Credentials.Dtos;
using Inventory.Configuration.Api.Application.Credentials.Services;
using Inventory.Configuration.Api.Application.Credentials.Validators;
using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
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
        public string Username { get; set; }
        public string Password { get; set; }
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
        private readonly IMapper _mapper;
        private readonly IAsyncRepository<Credential> _repository;


        public UpdateCredentialHanlder(ILogger<UpdateCredentialHanlder> logger, IMapper mapper, IAsyncRepository<Credential> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _repository = Guard.Against.Null(repository, nameof(repository));

        }

        public async Task<Payload<CredentialDto>> Handle(UpdateCredentialRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating Credential with id '{request.Id}'");

            // Find entity
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (null == entity)
                return Payload<CredentialDto>.Error(new NotFoundError($"Don't find Credential with Id {request.Id}"));

            // Update entity
            var nbChanges = await _repository.UpdateAsync(entity, cancellationToken);
            if (nbChanges > 0)
                _logger.LogInformation($"Successfully updated Credential with id '{request.Id}'");

            // return response
            var resultDto = _mapper.Map<CredentialDto>(entity);
            return Payload<CredentialDto>.Success(_mapper.Map<CredentialDto>(entity));

        }
    }

}
