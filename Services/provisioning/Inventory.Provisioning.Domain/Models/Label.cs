using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.Domain.Models
{
    public class Label : AuditEntity
    {
        public LabelName Name { get; private set; }
        public string Value { get; private set; }
    }
}
