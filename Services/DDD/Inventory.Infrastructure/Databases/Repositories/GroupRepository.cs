using Inventory.Domain.Models;
using Inventory.Domain.Repositories;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Databases.Repositories
{
    public class GroupRepository : EfRepository<Group>, IGroupRepository
    {
        public GroupRepository(InventoryDbContext dbContext) : base(dbContext) { }


        public List<Group> GetAllLinkedGroups(String groupName)
        {
            var result = new List<Group>();
            var groupTask = this.FirstAsync(new GroupSpecification(groupName));
            groupTask.Wait();
            result.Add(groupTask.Result);

            //var parents = this.GetParentGroups(groupName);
            var childrens = this.GetChildrenGroups(groupName);

            return result.Concat(childrens).Distinct().ToList();

        }

        public List<Group> GetParentGroups(String groupName)
        {
            var query = @$"
WITH RECURSIVE parent_groups AS (
	SELECT
		""GroupId"",

        ""ParentId"",
		""Name"",
        ""AnsibleGroupName"",
        ""xmin""

    FROM

        ""Group""

    WHERE

        ""Name"" = '{groupName}'

    UNION
        SELECT

            g.""GroupId"",
			g.""ParentId"",
    		g.""Name"",
            g.""AnsibleGroupName"",
            g.""xmin""

        FROM

            ""Group"" g
        INNER JOIN parent_groups s ON s.""ParentId"" = g.""GroupId""
) SELECT
    *
FROM
    parent_groups;
";

            return _dbContext.Groups.FromSqlRaw(query).ToList();

        }

        public List<Group> GetChildrenGroups(String groupName)
        {
            var query = @$"
WITH RECURSIVE parent_groups AS (
	SELECT
		""GroupId"",

        ""ParentId"",
		""Name"",
        ""AnsibleGroupName"",
        ""xmin""

    FROM

        ""Group""

    WHERE

        ""Name"" = '{groupName}'

    UNION
        SELECT

            g.""GroupId"",
			g.""ParentId"",
    		g.""Name"",
            g.""AnsibleGroupName"",
            g.""xmin""

        FROM

            ""Group"" g
        INNER JOIN parent_groups s ON s.""GroupId"" = g.""ParentId""
) SELECT
    *
FROM
    parent_groups;
";

            return _dbContext.Groups.FromSqlRaw(query).ToList();

        }



    }
}
