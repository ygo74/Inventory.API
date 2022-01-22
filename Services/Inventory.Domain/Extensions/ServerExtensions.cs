//using Inventory.Domain.Models;
//using Inventory.Domain.Models.ManagedEntities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Inventory.Domain.Extensions
//{
//    public static class ServerExtensions
//    {
//        public static IEnumerable<Group> GetInternalServerGroups(this Server server)
//        {
//            // Return ServerGroups link
//            foreach (ServerGroup sg in server.ServerGroups)
//            {
//                foreach (Group group in sg.Group.TraverseParents().Reverse())
//                {
//                    yield return group;
//                }
//            }
//        }
//    }
//}
