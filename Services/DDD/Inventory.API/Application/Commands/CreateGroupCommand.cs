using Inventory.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Application.Commands
{
    public class CreateGroupCommand : IRequest<Group>
    {
        public string Name { get; set; }
        public string AnsibleGroupName { get; set; }
        public string ParentName { get; set; }

    }
}
