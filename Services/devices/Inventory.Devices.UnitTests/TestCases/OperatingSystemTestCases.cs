using Inventory.Devices.Api.Applications.OperatingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Devices.UnitTests.TestCases
{
    static class OperatingSystemTestCases
    {

        public static IEnumerable GetOperatingSystemWithMissingMandatoryValues()
        {
            yield return new CreateOperatingSystem.Command2()
            {
                Model = "xxx",
                Version = "xxx"
            };
            yield return new CreateOperatingSystem.Command2()
            {
                OperatingSystemFamily = Api.Applications.OperatingSystem.Dto.OperatingSystemFamilyDto.Aix,
                Version = "xxx"
            };
            yield return new CreateOperatingSystem.Command2()
            {
                OperatingSystemFamily = Api.Applications.OperatingSystem.Dto.OperatingSystemFamilyDto.Aix,
                Model="",
                Version = "xxx"
            };
            yield return new CreateOperatingSystem.Command2()
            {
                OperatingSystemFamily = Api.Applications.OperatingSystem.Dto.OperatingSystemFamilyDto.Aix,
                Model = "xxx",
            };
            yield return new CreateOperatingSystem.Command2()
            {
                OperatingSystemFamily = Api.Applications.OperatingSystem.Dto.OperatingSystemFamilyDto.Aix,
                Model="xxx",
                Version = ""
            };

        }

        public static IEnumerable GetOperatingSystemWithBadValues()
        {
            yield return new CreateOperatingSystem.Command2()
            {
                OperatingSystemFamily = Api.Applications.OperatingSystem.Dto.OperatingSystemFamilyDto.Aix,
                Model = "Aix",
                Version = "10",
                ValidTo = DateTime.Today.AddDays(-1)
            };
            yield return new CreateOperatingSystem.Command2()
            {
                OperatingSystemFamily = Api.Applications.OperatingSystem.Dto.OperatingSystemFamilyDto.Aix,
                Model = "Aix",
                Version = "10",
                ValidFrom = DateTime.Today,
                ValidTo = DateTime.Today.AddDays(-1)
            };
        }


        public static IEnumerable GetOperatingSystemWithValidValues()
        {
            yield return new CreateOperatingSystem.Command2()
            {
                OperatingSystemFamily = Api.Applications.OperatingSystem.Dto.OperatingSystemFamilyDto.Windows,
                Model = "Server",
                Version = "2012"
            };
        }


    }
}
