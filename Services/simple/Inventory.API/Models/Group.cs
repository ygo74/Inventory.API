using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Models
{
    public class Group
    {

        public int GroupId { get; set; }

        [Required]
        public String Name { get; set; }

        public IList<ServerGroup> ServerGroups { get; set; }
    }
}
