using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.Credentials
{
    public class BasicCredential : Credential
    {
        public string Password { get; private set; }

        protected BasicCredential() : base() { }

        public BasicCredential(string name, string description) : base(name, description)
        {
        }

        public void Setpassword(string password)
        {
            Password = !string.IsNullOrWhiteSpace(password) ? password : throw new ArgumentNullException(nameof(password));
        }

    }
}
