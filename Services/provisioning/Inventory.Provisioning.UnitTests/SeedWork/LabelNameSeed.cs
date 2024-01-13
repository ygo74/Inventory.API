using Inventory.Provisioning.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.UnitTests.SeedWork
{
    public static class LabelNameSeed
    {
        public const string LABEL_OS_FAMILY = "os_family";
        public const string LABEL_OS = "os";

        public static IEnumerable<LabelName> Get()
        {
            yield return new LabelName(LABEL_OS_FAMILY);
            yield return new LabelName(LABEL_OS);
        }
    }
}
