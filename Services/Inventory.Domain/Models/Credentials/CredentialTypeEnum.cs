using Inventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Models.Credentials
{
    public class CredentialTypeEnum : Enumeration
    {
        public static CredentialTypeEnum Basic = new(0, nameof(Basic));
        public static CredentialTypeEnum Azure = new(1, nameof(Azure));



        public CredentialTypeEnum(int id, string name) : base(id, name) { }

    }
}
