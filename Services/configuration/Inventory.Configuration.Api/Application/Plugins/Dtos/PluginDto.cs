using HotChocolate;
using HotChocolate.Types;
using Inventory.Common.Application.Dto;
using Inventory.Common.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Plugins.Dtos
{
    public class PluginDto : ConfigurationEntityDto
    {
        private readonly Dictionary<string, object> _capacities = new Dictionary<string, object>();
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

        [GraphQLType(typeof(AnyType))]
        public IReadOnlyDictionary<string, object> Capacities
        {
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
