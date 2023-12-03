using Inventory.Common.Application.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Users
{
    public class UnAuthorisedError : GenericApiError
    {
        public UnAuthorisedError()
        {
            this.Message = "Unauthorised to process or access resource";
        }

        public UnAuthorisedError(string s)
        {

            this.Message = s;
        }

        public UnAuthorisedError(object content, string message)
        {

            this.Message = message;
        }

    }
}
