using FluentValidation;
using Inventory.Configuration.Api.Application.Datacenters.Dtos;
using Inventory.Configuration.Api.Application.Datacenters.Services;

namespace Inventory.Configuration.Api.Application.Datacenters.Validators
{
    public class DatacenterExistByIdvalidator : AbstractValidator<IDatacenterId>
    {
        public DatacenterExistByIdvalidator(IDatacenterService datacenterService)
        {
            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory")
                .MustAsync(datacenterService.DatacenterExists).WithMessage("Datacenter with {PropertyName} {PropertyValue} doesn't exists in the database");

        }
    }
}
