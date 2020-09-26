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

        private readonly IInventoryRepository _repository;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IInventoryRepository repository, ILogger<InventoryService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(_repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        public Task<Server> GetByIdAsync(Int64 id)
        {
            _logger.LogDebug($"Get Server by id : '{id}'");
            var server = _repository.GetAsync(id);

            return server;
        }
    }
}
