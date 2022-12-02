using Inventory.Common.Application.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Exceptions
{
    public class NotFoundError : GenericApiError
    {
        public NotFoundError()
        {
            this.message = "Not found entity in database";
        }

        public NotFoundError(string s)
        {
            this.message = s;
        }

        public NotFoundError(string propName, string message)
        {
            this.message = message;
            this.FieldName = propName;
        }

#nullable enable
        public string? FieldName { get; set; }
#nullable disable

    }
}
