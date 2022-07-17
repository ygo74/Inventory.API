using Inventory.Api.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Exceptions
{
    public class ValidationError : ApiError
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
