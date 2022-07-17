using Inventory.Api.Base.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Users
{
    public class UnAuthorisedError : ApiError
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
