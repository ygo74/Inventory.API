using Ardalis.GuardClauses;
using Inventory.Common.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Models
{
    public abstract class ConfigurationEntity : AuditEntity
    {
        public bool Deprecated { get; protected set; }

        public DateTime StartDate { get; protected set; }
        public DateTime? EndDate { get; protected set; }

        public string InventoryCode { get; protected set; }

        protected ConfigurationEntity(bool? deprecated = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            // configuration entity properties
            InventoryCode = string.Empty;
            Deprecated = deprecated.HasValue ? deprecated.Value : false;
            SetAvailabilityDate(startDate, endDate);
        }

        protected ConfigurationEntity(string inventoryCode, bool? deprecated = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            // configuration entity properties
            InventoryCode = Guard.Against.NullOrWhiteSpace(inventoryCode, nameof(inventoryCode)); ;
            Deprecated = deprecated.HasValue ? deprecated.Value : false;
            SetAvailabilityDate(startDate, endDate);
        }

        public void SetAvailabilityDate(DateTime? startDate = null, DateTime? endDate = null)
        {

            // Default start date is the date of creation
            StartDate = startDate.HasValue ? startDate.Value : DateTime.Today.ToUniversalTime();

            if (endDate.HasValue && endDate.Value.CompareTo(StartDate) < 0)
            {
                throw new InvalidDataException("End date {0} can't be before start date {1}", endDate.Value, startDate);
            }

            EndDate = endDate.HasValue ? endDate.Value.ToUniversalTime() : endDate;


        }

        public void SetDeprecatedValue(bool value)
        {
            Deprecated = value;
        }

        public void SetInventoryCode(string inventoryCode)
        {
            InventoryCode = Guard.Against.NullOrWhiteSpace(inventoryCode, nameof(inventoryCode));
        }

    }
}
