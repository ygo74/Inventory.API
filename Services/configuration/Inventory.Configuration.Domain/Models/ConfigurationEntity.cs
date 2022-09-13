using Inventory.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Models
{
    public abstract class ConfigurationEntity : AuditEntity
    {
        protected ConfigurationEntity(DateTime? validFrom, DateTime? validTo)
        {

            if (!validFrom.HasValue)
            {
                ValidFrom = DateTime.Today;
            }
            else
            {
                ValidFrom = new DateTime(validFrom.Value.Year, validFrom.Value.Month, validFrom.Value.Day);
            }

            if (validTo.HasValue)
            {
                ValidTo = new DateTime(validTo.Value.Year, validTo.Value.Month, validTo.Value.Day);
            }

        }

        public virtual DateTime? ValidFrom { get; protected set; }
        public virtual DateTime? ValidTo { get; protected set; }

        public virtual string InventoryCode { get; protected set; }
    }
}
