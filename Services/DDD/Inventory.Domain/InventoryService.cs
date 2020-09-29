using Inventory.Domain.Models;
using Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Inventory.Domain
{
    public class InventoryService
    {

        private readonly IServerRepository _serverRepository;
        private readonly IGroupRepository  _groupRepository;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IServerRepository serverRepository, 
                                IGroupRepository groupRepository, 
                                ILogger<InventoryService> logger)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public Task<Server> GetByIdAsync(Int64 id)
        {
            _logger.LogDebug($"Get Server by id : '{id}'");
            var server = _serverRepository.GetByIdAsync(id);

            return server;
        }

        public Task<Group> GetGroupByIdAsync(int id)
        {
            _logger.LogDebug($"Get Group by id : '{id}'");
            var group = _groupRepository.GetByIdAsync(id);

            return group;
        }
    }
}
