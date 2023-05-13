using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Application.Configuration.Base
{
    public class ConfigurationEntityValidator<T> : AbstractValidator<CreateConfigurationEntity<T>> where T : ConfigurationEntityDto
    {
        public ConfigurationEntityValidator()
        {
            When(e => e.ValidTo.HasValue && e.ValidFrom.HasValue, () =>
            {
                RuleFor(e => e.ValidTo).Must((model, validTo) =>
                {
                    return validTo.Value.CompareTo(model.ValidFrom.Value) > 0;
                }).WithMessage("{PropertyName} with {PropertyValue} must be greather than ValidFrom date");
            });
        }
    }
}
