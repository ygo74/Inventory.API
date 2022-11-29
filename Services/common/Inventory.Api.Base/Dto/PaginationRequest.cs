using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Dto
{
    public class PaginationRequest<T> : IRequest<T>
    {
        public string? After { get; set; }
        public string? Before { get; set; }
        public int? First { get; set; }
        public int? Last { get; set; }
        public int? Size { get; set; }
    }

    public class ConfigurationEntityRequest<T> : PaginationRequest<T>
    {
        public bool IncludeDeprecated { get; set; }
        public bool AllEntities { get; set; }
    }
}
