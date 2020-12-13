using Inventory.Domain.Models;
using System.Collections.Generic;

namespace Inventory.API.Application.Dto
{
    public class InventoryDto
    {

        public List<Group> Groups { get; set; }

        public List<ServerDto> Servers { get; set; }

    }
}
