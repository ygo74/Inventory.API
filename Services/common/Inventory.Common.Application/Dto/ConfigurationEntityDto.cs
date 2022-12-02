using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Dto
{
    public abstract class ConfigurationEntityDto : AuditEntityDto
    {
        public int Id { get; set; }
        public bool Deprecated { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
