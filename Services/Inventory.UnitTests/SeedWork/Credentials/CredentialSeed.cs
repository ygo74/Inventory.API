using Inventory.Domain.Models.Credentials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.UnitTests.SeedWork.Credentials
{
    public class CredentialSeed
    {
        public static List<Credential> Get()
        {
            var creds = new List<Credential>();

            var cred1 = new BasicCredential("user1", "Basic credential");
            cred1.Setpassword("testuser1");

            var cred2 = new AzureCredential("user2", "Azure credential");
            cred2.SetAzurePassword(Guid.Parse("f0d23385-1a8b-461a-a5f8-9ff7765ea945"), Guid.Parse("d61ca880-0c8b-4b7d-820d-c3e7018072d2"),
                                   Guid.Parse("5f4dc66b-1b0d-486b-8c4e-e05143ed896c"), "testuser2");


            creds.Add(cred1);
            creds.Add(cred2);

            return creds;
        }
    }
}
