using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
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
    /// Create credential request
    /// </summary>
    public class CreateCredentialRequest : IRequest<Payload<CredentialDto>>
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
    }

    /// <summary>
    /// Create credential request validator
    /// </summary>
    public class CreateCredentialRequestValidator : AbstractValidator<CreateCredentialRequest>
    {
        public CreateCredentialRequestValidator() 
        {
            RuleFor(e => e.Name).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");

        }
    }

    /// <summary>
    /// Create credential request handler
    /// </summary>
    public class CreateCredentialHanlder : IRequestHandler<CreateCredentialRequest, Payload<CredentialDto>>
    {
        private readonly ILogger<CreateCredentialHanlder> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public CreateCredentialHanlder(ILogger<CreateCredentialHanlder> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));

        }

        public async Task<Payload<CredentialDto>> Handle(CreateCredentialRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start adding Credential '{request.Name}'");

            bool success = false;
            try
            {
                var newEntity = new Credential(request.Name, request.Description);

                // Add entity
                await using var dbContext = _factory.CreateDbContext();
                var result = await dbContext.Credentials.AddAsync(newEntity, cancellationToken);
                var nbChanges = await dbContext.SaveChangesAsync(cancellationToken);

                // Map response
                var resultDto = _mapper.Map<CredentialDto>(newEntity);

                success = true;
                return Payload<CredentialDto>.Success(resultDto);
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully adding Credential '{request.Name}'");
                else
                    _logger.LogInformation($"Error when adding Credential '{request.Name}'");
            }
        }
    }

}
