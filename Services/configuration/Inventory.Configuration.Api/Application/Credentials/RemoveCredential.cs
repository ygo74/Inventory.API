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
    /// Remove Credential Request
    /// </summary>
    public class RemoveCredentialRequest : IRequest<Payload<CredentialDto>>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Remove credential request validator
    /// </summary>
    public class RemoveCredentialRequestValidator : AbstractValidator<RemoveCredentialRequest>
    {
        public RemoveCredentialRequestValidator(CredentialService service) 
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
    /// Remove Credential handler
    /// </summary>
    public class RemoveCredentialHanlder : IRequestHandler<RemoveCredentialRequest, Payload<CredentialDto>>
    {
        private readonly ILogger<RemoveCredentialHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public RemoveCredentialHanlder(ILogger<RemoveCredentialHanlder> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));

        }

        public async Task<Payload<CredentialDto>> Handle(RemoveCredentialRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start removing Credential with id '{request.Id}'");

            bool success = false;
            try
            {
                // Find entity
                await using var dbContext = _factory.CreateDbContext();
                var entity = await dbContext.Credentials.FindAsync(new object[] { request.Id }, cancellationToken);

                if (null == entity)
                    return Payload<CredentialDto>.Error(new NotFoundError($"Don't find Credential with Id {request.Id}"));

                // Remove entity
                dbContext.Credentials.Remove(entity);
                await dbContext.SaveChangesAsync(cancellationToken);

                success = true;
                return Payload<CredentialDto>.Success(default(CredentialDto));
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully removing Credential with id '{request.Id}'");
                else
                    _logger.LogInformation($"Error when removing Credential '{request.Id}'");
            }
        }
    }

}
