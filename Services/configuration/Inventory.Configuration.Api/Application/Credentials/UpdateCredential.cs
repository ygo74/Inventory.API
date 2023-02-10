using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Exceptions;
using Inventory.Configuration.Api.Application.Locations;
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
    public class UpdateCredentialRequest : IRequest<Payload<CredentialDto>>
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
        public UpdateCredentialRequestValidator(CredentialService service)
        {
            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory")
                .MustAsync(service.CredentialExists).WithMessage("Credential with {PropertyName} {PropertyValue} doesn't exists in the database");
        }
    }

    /// <summary>
    /// Update credential request handler
    /// </summary>
    public class UpdateCredentialHanlder : IRequestHandler<UpdateCredentialRequest, Payload<CredentialDto>>
    {
        private readonly ILogger<UpdateCredentialHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public UpdateCredentialHanlder(ILogger<UpdateCredentialHanlder> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));

        }

        public async Task<Payload<CredentialDto>> Handle(UpdateCredentialRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start updating Credential with id '{request.Id}'");

            bool success = false;
            try
            {
                // Find entity
                await using var dbContext = _factory.CreateDbContext();
                var entity = await dbContext.Credentials.FindAsync(new object[] { request.Id }, cancellationToken);

                if (null == entity)
                    return Payload<CredentialDto>.Error(new NotFoundError($"Don't find Credential with Id {request.Id}"));

                // Updat entity
                var nbChanges = await dbContext.SaveChangesAsync(cancellationToken);

                // Map response
                var resultDto = _mapper.Map<CredentialDto>(entity);

                success = true;
                return Payload<CredentialDto>.Success(resultDto);
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully updating Credential with id '{request.Id}'");
                else
                    _logger.LogInformation($"Error when updating Credential '{request.Id}'");
            }
        }
    }

}
