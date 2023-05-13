using Ardalis.GuardClauses;
using AutoMapper;
using FluentValidation;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Exceptions;
using Inventory.Common.Application.Validators;
using Inventory.Configuration.Api.Application.Locations;
using Inventory.Configuration.Api.Application.Plugin;
using Inventory.Configuration.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Inventory.Configuration.Api.Application.Plugins
{

    /// <summary>
    /// Remove plugin request
    /// </summary>
    public class RemovePluginRequest : DeleteConfigurationEntityRequest<PluginDto> { }

    /// <summary>
    /// Remove plugin request validator
    /// </summary>
    public class RemovePluginValidator : AbstractValidator<RemovePluginRequest>
    {

        public RemovePluginValidator(PluginService service)
        {

            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory and must be greather than 0")
                .MustAsync(service.PluginExist).WithMessage("Plugin with id {PropertyValue} doesn't exist");
        }

    }

    /// <summary>
    /// Remove plugin Handler
    /// </summary>
    public class RemovePluginHandler : IRequestHandler<RemovePluginRequest, Payload<PluginDto>>
    {

        private readonly ILogger<RemovePluginHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IDbContextFactory<ConfigurationDbContext> _factory;

        public RemovePluginHandler(ILogger<RemovePluginHandler> logger, IMapper mapper, IDbContextFactory<ConfigurationDbContext> factory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _factory = Guard.Against.Null(factory, nameof(factory));
        }

        /// <summary>
        /// Remove location
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Payload<PluginDto>> Handle(RemovePluginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Start deleting plugin '{request.Id}'");

            bool success = false;
            try
            {
                // Find entity
                await using var dbContext = _factory.CreateDbContext();
                var entity = await dbContext.Plugins.FindAsync(new object[] { request.Id },cancellationToken);

                if (null == entity)
                    return Payload<PluginDto>.Error(new NotFoundError($"Don't find plugin with Id {request.Id}"));

                // delete location
                dbContext.Plugins.Remove(entity);
                var changes = await dbContext.SaveChangesAsync(cancellationToken);

                if (changes <= 0)
                    return Payload<PluginDto>.Error();

                success = true;
                return Payload<PluginDto>.Success(default(PluginDto));
            }
            finally
            {
                if (success)
                    _logger.LogInformation($"Successfully deleting Plugin '{request.Id}'");
                else
                    _logger.LogInformation($"Error when deleting Plugin '{request.Id}'");
            }
        }
    }


}
