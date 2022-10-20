using FluentValidation;
using Inventory.Api.Base.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Validators
{
    public class ConfigurationEntityDtoValidator<T> : AbstractValidator<ICreateConfigurationEntityDto>
    {
        public ConfigurationEntityDtoValidator()
        {
            When(e => e.ValidTo.HasValue, () =>
            {
                RuleFor(e => e.ValidTo).Must((model, validTo) =>
                {
                    var ValidFrom = model.ValidFrom.HasValue ? model.ValidFrom.Value : DateTime.Today;

                    return validTo.Value.CompareTo(ValidFrom) > 0;
                }).WithMessage("{PropertyName} with {PropertyValue} must be greather than ValidFrom date");
            });

        }
    }
}
