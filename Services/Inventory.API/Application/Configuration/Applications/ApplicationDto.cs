using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Application.Configuration.Applications
{
    public class ApplicationDto : ConfigurationEntityDto
    {
        public string Name { get; private set; }
        public string Code { get; private set; }

    }
}
