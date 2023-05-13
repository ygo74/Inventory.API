using Inventory.API.Application.Configuration.Applications;
using Inventory.Domain.Models.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.SeedWork.Configuration
{
    internal class ApplicationSeed
    {
        internal const string APPLICATION1_NAME = "Application1";
        internal const string APPLICATION1_CODE = "APP1";

        internal const string APPLICATION2_NAME = "Application2";
        internal const string APPLICATION2_CODE = "APP2";

        internal static List<Application> Get()
        {
            return new List<Application>()
            {
                new Application(APPLICATION1_NAME, APPLICATION1_CODE),
                new Application(APPLICATION2_NAME, APPLICATION2_CODE)
            };
        }

        internal static class TestCases
        {
            public static IEnumerable GetCreateApplicationWithEmptyMandatoryValues()
            {
                yield return new CreateApplication.Command() { Name = "", Code = "" };
                yield return new CreateApplication.Command() { Name = "   ", Code = "  "};
                yield return new CreateApplication.Command() { Name = null, Code = null};
            }

            public static IEnumerable GetCreateApplicationWithValidValues()
            {
                yield return new CreateApplication.Command() { Name = "Application3", Code = "APP3" };
                yield return new CreateApplication.Command() { Name = "Application4", Code = "APP4", ValidFrom = DateTime.Now };
                yield return new CreateApplication.Command() { Name = "Application5", Code = "APP5", ValidFrom = DateTime.Now, ValidTo = DateTime.Now.AddYears(1) };
                yield return new CreateApplication.Command() { Name = "Application6", Code = "APP6", ValidTo = DateTime.Now.AddYears(1) };
            }

        }
    }
}
