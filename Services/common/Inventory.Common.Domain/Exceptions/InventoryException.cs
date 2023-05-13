using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Domain.Exceptions
{
    public class InventoryException : Exception
    {
        public InventoryException(string title, string message)
           : base(message) =>
           Title = title;


        public InventoryException(string title, string message, Exception innerException)
            : base(message, innerException) =>
            Title = title;

        protected InventoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public string Title { get; private set; }
    }
}
