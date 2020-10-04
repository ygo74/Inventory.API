using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class ServerGroup
    {
        public int ServerId { get; set; }
        public Server Server { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
