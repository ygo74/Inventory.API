using FluentValidation;
using Inventory.API.Application.Commands;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;

namespace Inventory.API.Commands
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
    {
        private readonly IAsyncRepository<Group> _groupRepository;

        public CreateGroupValidator(IAsyncRepository<Group> groupRepository)
        {

            _groupRepository = groupRepository;

            //Validate Group Name is not null and doesn't already exists in the database
            RuleFor(cg => cg.Name)
                .NotNull().NotEmpty().WithErrorCode("GRP-01")
                .MustAsync(async (GroupName, cancellation) =>
                {
                    var existingGroup = await _groupRepository.FirstOrDefaultAsync(new GroupSpecification(GroupName));
                    return (existingGroup == null);
                }).WithMessage("'{PropertyName}' Must be unique in the database").WithErrorCode("GRP-02");


            //Validate Ansible group Name has the correct syntax
            RuleFor(cg => cg.AnsibleGroupName).Must(ansibleGroupName =>
            {
                if (string.IsNullOrWhiteSpace(ansibleGroupName)) return true;

                //TODO : Use a full regex to validate the ansible syntax
                return (!ansibleGroupName.Contains(" ") & !ansibleGroupName.Contains("-"));

            }).WithMessage("'{PropertyName}' Must be a valid Ansible Group syntax").WithErrorCode("GRP-03");

            //Validate Parent exists in the database
            RuleFor(cg => cg.ParentName).MustAsync(async (parentGroupName, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(parentGroupName)) return true;
                var existingGroup = await _groupRepository.FirstOrDefaultAsync(new GroupSpecification(parentGroupName));
                return (existingGroup != null);

            }).WithMessage("'{PropertyName}' Must exists in the Database").WithErrorCode("GRP-04");

        }
    }
}
