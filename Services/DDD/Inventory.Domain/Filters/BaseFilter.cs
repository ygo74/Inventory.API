using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Filters
{
    public class BaseFilter
    {
        public bool LoadChildren { get; set; }
        public bool IsPagingEnabled { get; set; }
        public bool IsCacheEnabled { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

    }
}
