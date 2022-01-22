using System;
using Environment = Inventory.Domain.Models.Configuration.Environment;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Domain.Enums;

namespace Inventory.UnitTests.SeedWork.Configuration
{
    public class EnvironmentSeed
    {

        public static List<Environment> Get()
        {
            return new List<Environment>()
            {
                new Environment("prd","Production", EnvironmentFamily.Production),
                new Environment("drp", "Disaster recovery", EnvironmentFamily.Production),
                new Environment("dev", "Development", EnvironmentFamily.Developoment),
                new Environment("sit", "SIT", EnvironmentFamily.Tests),
                new Environment("uat", "User Acceptance Tests", EnvironmentFamily.Tests),
                new Environment("poc", "Proof of concept", EnvironmentFamily.Developoment)

            };
        }
    }
}
