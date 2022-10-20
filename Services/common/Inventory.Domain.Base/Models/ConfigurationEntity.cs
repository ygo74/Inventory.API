using Inventory.Domain.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Base.Models
{
    public abstract class ConfigurationEntity : AuditEntity
    {
        public bool Deprecated { get; protected set; }

        public DateTime StartDate { get; protected set; }
        public DateTime? EndDate { get; protected set; }

        public void SetAvailabilityDate(DateTime? startDate=null, DateTime? endDate=null)
        {

            // Default start date is the date of creation
            StartDate = startDate.HasValue ? startDate.Value : DateTime.Today;

            if (endDate.HasValue && endDate.Value.CompareTo(StartDate) < 0)
            {
                throw new InvalidDataException("End date {0} can't be before start date {1}", endDate.Value, startDate);
            }
            EndDate = endDate;

        }

    }
}
