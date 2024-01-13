using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.Domain.Models
{
    public class LabelName : ConfigurationEntity
    {
        public string Name { get; private set; }

        public LabelName(string name) 
        { 
            Name = name;
        }


    }
}
