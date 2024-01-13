using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Errors
{
    public class NotFoundError : GenericApiError
    {
        public NotFoundError()
        {
            Message = "Not found entity in database";
        }

        public NotFoundError(string message)
        {
            Message = message;
        }

        public NotFoundError(string propName, string message)
        {
            this.Message = message;
            FieldName = propName;
        }

#nullable enable
        public string? FieldName { get; set; }
#nullable disable

    }
}
