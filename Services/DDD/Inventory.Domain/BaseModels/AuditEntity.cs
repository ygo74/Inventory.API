using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.BaseModels
{
    public class AuditEntity : Entity
    {
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedBy { get; set; }

    }
}
