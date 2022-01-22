using Ardalis.Specification;
using Inventory.Domain.Models;
using Inventory.Domain.Models.ManagedEntities;
using Inventory.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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


        public Specification<Server> ToSpecification()
        {
            var spec = new ServerSpecification();

            if (!string.IsNullOrEmpty(Hostname))
            {
                spec.And(new ServerSpecification(Hostname));
            }

            return spec;
        }

    }
}
