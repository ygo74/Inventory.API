using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Models
{
    public class Server
    {

        public int ServerId { get; set; }

        [Required]
        public string Name { get; set; }

        public OsType OperatingSystem { get; set; }

        public IList<ServerGroup> ServerGroups { get; set; }

    }
}
