using Inventory.Common.DomainModels;
using Inventory.Domain.Enums;
using Inventory.Domain.Models.ManagedEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.Configuration
{
    public class DataCenter : ConfigurationEntity
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public DataCenterType DataCenterType { get; private set; }


        // Server links
        private List<Server> _servers = new List<Server>();
        public ICollection<Server> Servers => _servers.AsReadOnly();

        public DataCenter(string code, string name, DataCenterType dataCenterType)
        {
            Code = !String.IsNullOrEmpty(code) ? code : throw new ArgumentNullException(nameof(code));
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));

            DataCenterType = dataCenterType;

        }
    }
}
