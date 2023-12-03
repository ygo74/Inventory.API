using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Errors
{
    public class ValidationError : GenericApiError
    {
        public ValidationError()
        {
            Message = "Some parameter/s are invalid or null";
        }

        public ValidationError(string message)
        {
            Message = message;
        }

        public ValidationError(string propName, string message)
        {
            this.Message = message;
            FieldName = propName;
        }

#nullable enable
        public string? FieldName { get; set; }
#nullable disable

    }
}
