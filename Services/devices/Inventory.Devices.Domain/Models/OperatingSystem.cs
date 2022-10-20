using Ardalis.GuardClauses;
using Inventory.Domain.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.Domain.Models
{
    public class OperatingSystem : ConfigurationEntity
    {
        public OperatingSystemFamily Family { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }

        protected OperatingSystem()
        { }

        public OperatingSystem(OperatingSystemFamily family, string model, string version, bool? deprecated=null, DateTime? startDate=null, DateTime? endDate=null )
        {
            // Os properties
            Family = Guard.Against.Null(family, nameof(family));
            Model = Guard.Against.NullOrWhiteSpace(model, nameof(model));
            Version = Guard.Against.NullOrWhiteSpace(version, nameof(version));

            // configuration entity properties
            Deprecated = deprecated.HasValue ? deprecated.Value : false;
            SetAvailabilityDate(startDate, endDate);

        }

    }
}
