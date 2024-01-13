using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Errors
{
    public interface IApiError
    {
        string Message { get; set; }
    }

    public class GenericApiError : IApiError
    {
        public GenericApiError()
        {
            Message = "Some error occured";
        }

        public GenericApiError(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
