using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Dto
{
    public class InventoryDto
    {

        public List<Group> Groups { get; set; }

        public List<ServerDto> Servers { get; set; }

    }
}
