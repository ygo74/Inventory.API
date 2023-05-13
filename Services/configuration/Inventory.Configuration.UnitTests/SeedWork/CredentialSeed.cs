using Inventory.Configuration.Domain.Models;
using Inventory.Configuration.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.UnitTests.SeedWork
{
    public static class CredentialSeed
    {

        public const string ADMINISTRATOR = "admin";

        public static IEnumerable<Credential> Get()
        {

            yield return new Credential(ADMINISTRATOR, "test azure");

        }

    }
}
