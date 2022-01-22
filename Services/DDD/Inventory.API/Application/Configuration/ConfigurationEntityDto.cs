using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Application.Configuration
{
    public class ConfigurationEntityDto
    {
        public int Id { get; private set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

    }
}
