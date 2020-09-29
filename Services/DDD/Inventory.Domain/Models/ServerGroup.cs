using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class ServerGroup
    {
        public Int64 ServerId { get; set; }
        public Server Server { get; set; }

        public Int64 GroupId { get; set; }
        public Group Group { get; set; }

    }
}
