using Ardalis.GuardClauses;
using Inventory.Common.Domain.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Inventory.Configuration.Domain.Models
{
    public class Location : ConfigurationEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public string CountryCode { get; private set; }
        public string CityCode { get; private set; }
        public string RegionCode { get; private set;}

        private List<Datacenter> _datacenters = new List<Datacenter>();
        public ICollection<Datacenter> Datacenters => _datacenters.AsReadOnly();


        protected Location()
        {
        }

        public Location(string name, string countryCode,string cityCode, string regionCode, string inventoryCode,
                        string description, bool? deprecated = null, DateTime? startDate = null, DateTime? endDate = null)
            : base(inventoryCode, deprecated, startDate, endDate)
        {
            // Mandatory
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            CountryCode = Guard.Against.NullOrWhiteSpace(countryCode, nameof(countryCode)).ToLower();
            CityCode = Guard.Against.NullOrWhiteSpace(cityCode, nameof(cityCode)).ToLower();
            RegionCode = Guard.Against.NullOrWhiteSpace(regionCode, nameof(regionCode)).ToLower();

            //Optional
            Description = description;
        }
    }
}
