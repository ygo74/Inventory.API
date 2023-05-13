using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Exceptions
{
    public class InvalidDataException : InventoryException
    {
        public InvalidDataException(string message) : base("Invalid data", message)
        {

        }

        public InvalidDataException(string message, params object[] values) : base("Invalid Data", string.Format(message, values))
        {            
        }


    }
}
