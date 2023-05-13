using Inventory.Common.Application.Core;
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
            this.message = "Unauthorised to process or access resource";
        }

        public UnAuthorisedError(string s)
        {

            this.message = s;
        }

        public UnAuthorisedError(object content, string message)
        {

            this.message = message;
        }

    }
}
