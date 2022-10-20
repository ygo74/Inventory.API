using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Base.Exceptions
{
    public class InvalidDataException : InventoryException
    {
        public InvalidDataException(string message) : base(message)
        {

        }

        public InvalidDataException(string message, params object[] values) : base(string.Format(message, values))
        {            
        }


    }
}
