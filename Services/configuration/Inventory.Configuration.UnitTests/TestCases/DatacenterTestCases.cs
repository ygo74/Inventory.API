using Inventory.Configuration.Api.Application.Datacenters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.TestCases
{
    internal static class DatacenterTestCases
    {
        internal static IEnumerable GetCreateDatacenterWithEmptyMandatoryValues()
        {
            yield return new CreateDatacenterRequest() { Name = "", Code = "" };
            yield return new CreateDatacenterRequest() { Name = "   ", Code = "  " };
            yield return new CreateDatacenterRequest() { Name = null, Code = null };
        }

        internal static IEnumerable GetCreateDatacenterWithCorrectMandatoryValues()
        {
            yield return new CreateDatacenterRequest() { Name = "TEST-XX", Code = "TEST-XX" };
        }

    }
}
