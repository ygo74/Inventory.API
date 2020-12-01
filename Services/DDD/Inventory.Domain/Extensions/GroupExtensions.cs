using Inventory.Domain.Models;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventory.Domain.Extensions
{
    public static class GroupExtensions
    {
        public static IEnumerable<Group> TraverseParents(this Group entity)
        {
            var stack = new Stack<Group>();
            stack.Push(entity);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                if (null != current.Parent)
                {
                    stack.Push(current.Parent);
                }

            }
        }

        /// <summary>
        /// Get group and all its children
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static IEnumerable<Group> FlattenChildrends(this Group entity)
        {

            // Init group with entity
            List<Group> allChildrendGroups = new List<Group>();

            // Add child group
            foreach(Group childGroup in entity.Children)
            {
                allChildrendGroups.Add(childGroup);
                if (childGroup.Children.Count() > 0)
                {
                    var subGroup = childGroup.FlattenChildrends();
                    allChildrendGroups.Concat(subGroup);
                }
            }

            return allChildrendGroups;
        }


    }
}
