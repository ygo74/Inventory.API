﻿using Ardalis.Specification;
using Inventory.Domain.Filters;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Domain.Specifications
{
    public class ServerSpecification : Specification<Server>
    {
        public ServerSpecification() 
        {
            Query.Include(s => s.OperatingSystem);
            Query.Include(s => s.ServerDisks);
            Query.Include(s => s.ServerEnvironments);
            Query.Include(s => s.ServerGroups).ThenInclude(sg => sg.Group);
        }

        public ServerSpecification(string hostName):this()
        {
            Query
                .Where(s => s.HostName == hostName.ToLower());
        }

        public ServerSpecification(int Id) : this()
        {
            Query
                .Where(s => s.ServerId == Id);
        }

        public ServerSpecification(string[] groupNames, string environment) : this()
        {
            Query.Where(s => s.ServerGroups.Any(sg => groupNames.Contains(sg.Group.Name)) 
                        && s.ServerEnvironments.Any(se => se.Environment.Name == environment));
        }

        /// <summary>
        /// Get multiple server according a list of server ids
        /// </summary>
        /// <param name="serverIds"></param>
        public ServerSpecification(IEnumerable<int> serverIds) : this()
        {
            Query.Where(s => serverIds.Contains(s.ServerId));
        }

        public ServerSpecification(ServerFilter filter) : this()
        {
            if (filter.LoadChildren)
            {
                Query.Include(s => s.OperatingSystem);
                Query.Include(s => s.ServerDisks);
                Query.Include(s => s.ServerEnvironments);
                Query.Include(s => s.ServerGroups).ThenInclude(sg => sg.Group);
            }

            if (filter.IsPagingEnabled)
            {
                Query.Skip(filter.Skip);
                Query.Take(filter.Take);
            }

            if (filter.EnvironmentIds != null)
            {
                Query.Include(s => s.ServerEnvironments).ThenInclude(se => se.Environment);
                Query.Where(s => s.ServerEnvironments.Any(se => filter.EnvironmentIds.Contains(se.EnvironmentId)));
                
            }
            
        }

    }
}
