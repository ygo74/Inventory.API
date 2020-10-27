using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

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
