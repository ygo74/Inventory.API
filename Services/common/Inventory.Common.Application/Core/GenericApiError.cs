using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Core
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
