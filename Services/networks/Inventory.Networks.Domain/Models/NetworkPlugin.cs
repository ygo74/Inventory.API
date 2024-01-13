using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Networks.Domain.Models
{
    public class NetworkPlugin
    {
        public string Name { get; private set; }
        public string Code { get; private set; }

        public string Path { get; private set; }

        protected NetworkPlugin() { }

    }
}
