using Inventory.Api.Base.Dto;
using Inventory.Api.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugin
{
    public class PluginDto : ConfigurationEntityDto
    {
        private readonly Dictionary<string, bool> _capacities = new Dictionary<string, bool>();
        public PluginDto()
        {
            _capacities.Add("SubnetProvider", false);
        }

        public string Name { get; set; }

        /// <summary>
        /// Test code description
        /// </summary>
        public string Code { get; set; }

        public string Version { get; set; }

        public Dictionary<string, bool> Capacities { 
            get { return _capacities; }
        }

        public void SetCapacity(string name, bool supported)
        {
            if (!_capacities.ContainsKey(name))
            {
                throw new InventoryApiException($"Unknown capacity {name}");
            }

            _capacities[name] = supported;
        }
    }
}
