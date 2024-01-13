using Ardalis.GuardClauses;
using FluentValidation;
using Inventory.Configuration.Api.Application.Credentials.Dtos;
using Inventory.Configuration.Api.Application.Credentials.Services;

namespace Inventory.Configuration.Api.Application.Credentials.Validators
{
    public class CredentialExistByIdValidator : AbstractValidator<ICredentialId>
    {
        public CredentialExistByIdValidator(ICredentialService service)
        {
            // Check that ICredentialService is not null
            Guard.Against.Null(service, nameof(service));

            RuleFor(e => e.Id).Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is mandatory")
                .MustAsync(service.CredentialExists).WithMessage("Credential with {PropertyName} {PropertyValue} doesn't exists in the database");
        }
    }
}
