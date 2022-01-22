using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Enums
{
    // https://ardalis.com/persisting-the-type-safe-enum-pattern-with-ef-6/
    public class LifecycleStatus : Enumeration
    {
        public static LifecycleStatus New = new(0, nameof(New));
        public static LifecycleStatus ToBeCreated = new(1, nameof(ToBeCreated));
        public static LifecycleStatus Created = new(2, nameof(Created));
        public static LifecycleStatus Deployed = new(3, nameof(Deployed));



        public LifecycleStatus(int id, string name) : base(id, name) { }
    }
}
