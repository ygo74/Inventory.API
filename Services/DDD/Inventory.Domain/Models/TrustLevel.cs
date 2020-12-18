using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Models
{
    public class TrustLevel
    {
        public int TrustLevelId { get; private set; }
        public string Name { get; private set; }
        public string Code { get; private set; }

        public TrustLevel(string name, string code)
        {
            Name = !String.IsNullOrEmpty(name) ? name : throw new ArgumentNullException(nameof(name));
            Code = !String.IsNullOrEmpty(code) ? code : throw new ArgumentNullException(nameof(code));
        }

    }
}
