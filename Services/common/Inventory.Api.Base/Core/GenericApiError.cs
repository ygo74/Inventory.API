using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Core
{
    public interface IApiError
    {
        string message { get; set; }
    }

    public class GenericApiError : IApiError
    {
        public string message { get; set; }
    }
}
