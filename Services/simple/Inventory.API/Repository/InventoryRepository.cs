using Inventory.Infrastructure;
using Inventory.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Repository
{

    public class InventoryRepository : IInventoryRepository, IUnitOfWork
    {
        private readonly InventoryDbContext _inventoryContext;

        public InventoryRepository(InventoryDbContext inventoryContext)
        {
            _inventoryContext = inventoryContext != null ? inventoryContext : throw new ArgumentNullException(nameof(inventoryContext));
        }

        #region IUnitOfWork

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _inventoryContext.SaveChangesAsync(cancellationToken);

            return result;

        }

        #endregion

        public async Task<IDictionary<int, Server>> GetServersByIdAsync(IEnumerable<int> serverIds, CancellationToken token)
        {
            return await _inventoryContext.Servers
                                          .Where(s => serverIds.Contains(s.ServerId))
                                          .ToDictionaryAsync(s => s.ServerId, cancellationToken: token);
        }

        /// <summary>
        /// Get Groups By Server
        /// </summary>
        /// <param name="serverIds"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<ILookup<int, Group>> GetgroupsByServerAsync(IEnumerable<int> serverIds, CancellationToken token)
        {
            var serverGroups = await _inventoryContext.ServerGroups
                                          .Where(sg => serverIds.Contains(sg.ServerId))
                                          .Include(sg => sg.Group).ToListAsync();
//                                          .Select(sg => sg.Group).ToListAsync();

            return serverGroups.ToLookup(s => s.ServerId, s => s.Group );
                                          //.ToDictionaryAsync(s => $"{s.GroupId}}", cancellationToken: token);
        }

        public async Task<ILookup<int, Server>> GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token)
        {
            var serverGroups = await _inventoryContext.ServerGroups
                                          .Where(sg => groupIds.Contains(sg.GroupId))
                                          .Include(sg => sg.Server).ToListAsync();
            //                                          .Select(sg => sg.Group).ToListAsync();

            return serverGroups.ToLookup(s => s.GroupId, s => s.Server);
            //.ToDictionaryAsync(s => $"{s.GroupId}}", cancellationToken: token);
        }

        public async Task<Group> GetGroupById(int groupId)
        {
            return await _inventoryContext.Groups.FirstOrDefaultAsync(g => g.GroupId == groupId);
        }


        /// <summary>
        /// Create a New Group in the inventory
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="ParentName"></param>
        /// <returns></returns>
        public Group CreateGroup(String Name, String ParentName=null)
        {
            Group parentGroup = null;
            if (!String.IsNullOrEmpty(ParentName))
            {
                parentGroup = _inventoryContext.ChangeTracker.Entries<Group>().Where(e => e.State == EntityState.Added).Select(grp => grp.Entity).SingleOrDefault(grp => grp.Name == ParentName.ToLower());
                if (null == parentGroup)
                {
                    parentGroup = _inventoryContext.Groups.SingleOrDefault(grp => grp.Name == ParentName.ToLower());
                    if (null == parentGroup)
                    {
                        throw new ArgumentException($"{ParentName} doesn't exists");
                    }
                }
            }

            Group newGroup = new Group(Name);
            if (null == parentGroup)
            {
                _inventoryContext.Groups.Add(newGroup);
            }
            else
            {
                parentGroup.AddSubGroups(newGroup);
            }

            return newGroup;
        }



        public List<Group> GetParentGroups(String groupName)
        {
            var query = @$"
WITH RECURSIVE parent_groups AS (
	SELECT
		""GroupId"",

        ""ParentId"",
		""Name"",
        ""AnsibleGroupName""

    FROM

        ""Group""

    WHERE

        ""Name"" = '{groupName}'

    UNION
        SELECT

            g.""GroupId"",
			g.""ParentId"",
    		g.""Name"",
            g.""AnsibleGroupName""

        FROM

            ""Group"" g
        INNER JOIN parent_groups s ON s.""ParentId"" = g.""GroupId""
) SELECT
    *
FROM
    parent_groups;
";

            return _inventoryContext.Groups.FromSqlRaw(query).ToList();

        }

        public List<Group> GetChildrenGroups(String groupName)
        {
            var query = @$"
WITH RECURSIVE parent_groups AS (
	SELECT
		""GroupId"",

        ""ParentId"",
		""Name"",
        ""AnsibleGroupName""

    FROM

        ""Group""

    WHERE

        ""Name"" = '{groupName}'

    UNION
        SELECT

            g.""GroupId"",
			g.""ParentId"",
    		g.""Name"",
            g.""AnsibleGroupName""

        FROM

            ""Group"" g
        INNER JOIN parent_groups s ON s.""GroupId"" = g.""ParentId""
) SELECT
    *
FROM
    parent_groups;
";

            return _inventoryContext.Groups.FromSqlRaw(query).ToList();

        }


    }
}
