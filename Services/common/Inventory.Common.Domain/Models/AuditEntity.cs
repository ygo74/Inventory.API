using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Models
{
    public abstract class AuditEntity : Entity
    {
        public DateTime Created { get; protected set; }
        public string CreatedBy { get; protected set; }

        public DateTime? LastModified { get; protected set; }
        public string LastModifiedBy { get; protected set; }

    }
}
