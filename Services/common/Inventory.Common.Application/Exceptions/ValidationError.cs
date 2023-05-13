using Inventory.Common.Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Exceptions
{
    public class ValidationError : GenericApiError
    {
        public ValidationError()
        {
            this.message = "Some parameter/s are invalid or null";
        }

        public ValidationError(string s)
        {
            this.message = s;
        }

        public ValidationError(string propName, string message)
        {
            this.message = message;
            this.FieldName = propName;
        }

#nullable enable
        public string? FieldName { get; set; }
#nullable disable

    }
}
