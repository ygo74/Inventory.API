using FluentValidation;
using Inventory.API.Application.Commands;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;

namespace Inventory.API.Commands
{
    public class CreateServerValidator : AbstractValidator<CreateServerCommand>
    {
        private readonly IAsyncRepository<Server> _serverRepository;

        public CreateServerValidator(IAsyncRepository<Server> serverRepository)
        {

            _serverRepository = serverRepository;

            RuleFor(cs => cs.HostName)
                .NotNull().NotEmpty().WithErrorCode("SRV-01")
                .MustAsync(async (hostname, cancellation) =>
                {
                    var existingServer = await _serverRepository.FirstOrDefaultAsync(new ServerSpecification(hostname));
                    return (existingServer == null);
                }).WithMessage("'{PropertyName}' Must be unique in the database").WithErrorCode("SRV-02");


            RuleFor(cs => cs.SubnetIp).Must(ip =>
            {
                System.Net.IPAddress iPAddress;
                return System.Net.IPAddress.TryParse(ip, out iPAddress);
            }).WithMessage("'{PropertyName}' Must be a valid IP").WithErrorCode("SRV-03");
        }
    }
}
