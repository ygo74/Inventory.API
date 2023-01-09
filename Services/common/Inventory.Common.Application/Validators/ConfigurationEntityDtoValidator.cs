﻿using FluentValidation;
using Inventory.Common.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Validators
{
    public class ConfigurationEntityDtoValidator<T> : AbstractValidator<T> where T : ICreateOrUpdateConfigurationEntityRequest
    {
        public ConfigurationEntityDtoValidator()
        {

            RuleFor(e => e.InventoryCode).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage("{PropertyName} is mandatory");


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
