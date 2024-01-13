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
        public const string TO_BE_DELETED = "to_be_deleted";

        public static IEnumerable<Credential> Get()
        {

            yield return new Credential(ADMINISTRATOR, "test azure");
            yield return new Credential(TO_BE_DELETED, "test to be deleted");

        }

    }
}
