using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Inventory.Domain.Models
{
    public class Group : Entity
    {
        //Group 
        public Int64 GroupId { get; set; }
        public String Name { get; set; }

        //Parent Group
        public Group  Parent { get; set; }
        public int?   ParentId { get; set; }

        // Children groups
        public ICollection<Group> Children { get; } = new List<Group>();

        // Servers
        public ICollection<ServerGroup> ServerGroups { get; set; }

        //public override int Id
        //{
        //    get { return GroupId; }
        //}
    }
}
