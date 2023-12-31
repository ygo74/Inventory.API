using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Core
{

    public interface IPaginationRequest
    {
        int Size { get; set; } 
    }

# nullable enable
    public class CursorPaginationRequest : IPaginationRequest
    {
        public string? After { get; set; }
        public string? Before { get; set; }
        public int Size { get; set; } = 50;
        public bool FirstPage { get; set; }
        public bool LasttPage { get; set; }
    }
# nullable disable

    public class OffsetPaginationRequest : IPaginationRequest
    {
        public int Size { get; set; } = 50;
        public int Skip { get; set; } = 0;
    }


}
