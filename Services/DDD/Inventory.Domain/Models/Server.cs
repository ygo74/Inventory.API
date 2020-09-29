using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class Server : Entity
    {
        public Int64 ServerId { get; set; }
        public string Name { get; set; }
        public OsType OperatingSystem { get; set; }

        public ICollection<ServerGroup> ServerGroups { get; set; }

        //public override int Id
        //{
        //    get { return this.ServerId; }
        //}
    }
}
