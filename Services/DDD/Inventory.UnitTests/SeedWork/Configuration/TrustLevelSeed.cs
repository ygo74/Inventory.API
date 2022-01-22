using Inventory.Domain.Models.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.SeedWork.Configuration
{
    public class TrustLevelSeed
    {
        public static List<TrustLevel> Get()
        {
            var trustLevels = new List<TrustLevel>();

            trustLevels.Add(new TrustLevel("Confidential", "CONF"));
            trustLevels.Add(new TrustLevel("Secret", "SEC"));
            trustLevels.Add(new TrustLevel("Extranet", "EXT"));
            trustLevels.Add(new TrustLevel("Public", "PUB"));

            return trustLevels;
        }

    }
}
