using Inventory.Domain.BaseModels;
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


        public DataCenter(string code, string name)
        {
            Code = !String.IsNullOrEmpty(code) ? code : throw new ArgumentNullException(nameof(code));
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));

        }
    }
}
