using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Exceptions
{
    public class InventoryApiException : Exception
    {
        public InventoryApiException()
        {
        }

        public InventoryApiException(string message) : base(message)
        {
        }

        public InventoryApiException(string message, Exception inner) : base(message, inner)
        {
        }

    }
}
