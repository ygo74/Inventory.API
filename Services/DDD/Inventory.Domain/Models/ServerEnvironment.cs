using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class ServerEnvironment
    {
        public int ServerId { get; set; }
        public Server Server { get; set; }

        public int EnvironmentId { get; set; }
        public Environment Environment { get; set; }

    }
}
