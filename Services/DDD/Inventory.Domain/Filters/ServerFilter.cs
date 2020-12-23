using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Filters
{
    public class ServerFilter : BaseFilter
    {

        public ServerFilter()
        {
        }

        public ServerFilter(bool loadChildren, bool enablePagination)
        {
            LoadChildren = loadChildren;
            IsPagingEnabled = enablePagination;
        }

        public Nullable<int> Id { get; set; }
        public string Environment { get; set; }
        public string Hostname { get; set; }
        public string GroupName { get; set; }

        public string[] GroupNames { get; set; }
        public int[] Ids { get; set; }
        public int[] EnvironmentIds { get; set; }

    }
}
