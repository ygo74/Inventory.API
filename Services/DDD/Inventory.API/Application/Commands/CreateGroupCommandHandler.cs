using Inventory.Domain;
using Inventory.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Application.Commands
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Group>
    {

        private readonly InventoryService _inventoryService;
        private readonly ILogger<CreateServerCommandHandler> _logger;

        public CreateGroupCommandHandler(InventoryService inventoryService, ILogger<CreateServerCommandHandler> logger)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public Task<Group> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var newGroup = _inventoryService.GetorAddGroupAsync(request.Name, request.ParentName, request.AnsibleGroupName);
            return newGroup;
        }
    }
}
