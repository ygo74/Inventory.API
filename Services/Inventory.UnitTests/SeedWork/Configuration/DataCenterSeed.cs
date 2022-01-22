using Inventory.Domain.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.SeedWork.Configuration
{
    public class DataCenterSeed
    {
        public static List<DataCenter> Get()
        {
            var datacenters = new List<DataCenter>();

            datacenters.Add(new DataCenter("EMEA-FR-PARIS","France"));
            datacenters.Add(new DataCenter("EMEA-UK-LDN", "United Kingdom"));

            return datacenters;
        }

    }
}
