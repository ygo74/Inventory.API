using Inventory.Configuration.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.SeedWork
{
    public static class DataCenterSeed
    {
        public static List<Datacenter> Get()
        {
            var datacenters = new List<Datacenter>();

            datacenters.Add(new Datacenter("EMEA-FR-PARIS", "France", DatacenterType.OnPremise));
            datacenters.Add(new Datacenter("EMEA-UK-LDN", "United Kingdom", DatacenterType.OnPremise));

            return datacenters;
        }

    }
}
