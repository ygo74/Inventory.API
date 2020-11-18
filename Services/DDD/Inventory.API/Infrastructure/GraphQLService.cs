using Inventory.API.Dto;
using Inventory.Domain;
using Inventory.Domain.Extensions;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Infrastructure
{
    public class GraphQLService : IGraphQLService
    {

        private readonly InventoryService _inventoryService;
        private readonly IGroupRepository _groupRepository;
        private readonly IAsyncRepository<Server> _serverRepository;
        private readonly IMemoryCache _cache;


        public GraphQLService(InventoryService inventoryService, IGroupRepository groupRepository, IAsyncRepository<Server> serverRepository, IMemoryCache cache)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        }


        #region "Servers"
        public async Task<ILookup<int, Server>> GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token)
        {
            var serverGroups = await _inventoryService.GetServersByGroupAsync(groupIds);
            return serverGroups.ToLookup(s => s.GroupId, s => s.Server);

        }

        #endregion

        #region "Groups : To do find all parent groups for servers"
        //public async Task<ILookup<Server, List<Group>>> GetParentGroupsByServer(IEnumerable<Server> servers, CancellationToken token)
        //{
        //    var allServerGroups = servers.Select(s => s.ServerGroups).ToList();
        //    List<String> groupNames = new List<string>();
        //    foreach (ServerGroup sg in allServerGroups)
        //    {
        //        groupNames.Add(sg.Group.AnsibleGroupName);
        //    }

        //    //var allGroups = _groupRepository.GetParentGroups()
        //    servers.ToLookup(s => s, s => s.ServerGroups )


        //}

        #endregion

        private async Task<T> UseCacheAsync<T>(Func<Task<T>> internalMethod, string cacheKey)
        {

            //Try to obtain the variables from the cache
            T outputValue;
            if (_cache.TryGetValue(cacheKey, out outputValue))
            {
                return outputValue;
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions();
            outputValue = await internalMethod.Invoke();
            _cache.Set(cacheKey, outputValue, cacheEntryOptions);

            return outputValue;

        }


        public async Task<IReadOnlyList<Group>> GetAllGroupAsync()
        {
            // Get AllGroups
            //var allInventoryGroups = await _groupRepository.ListAsync(new GroupSpecification());
            //return allInventoryGroups;

            var result = await UseCacheAsync<IReadOnlyList<Group>>(async () =>
            {
                // Get AllGroups
                var allInventoryGroups = await _groupRepository.ListAsync(new GroupSpecification());
                return allInventoryGroups;
            }, "test");


            return result;
        }



        public async Task<InventoryDto> GetInventoryForGroupAsync(string groupName, string environment)
        {
            // Get AllGroups
            var allInventoryGroups = await this.GetAllGroupAsync();

            // Find all Allowed groups and child groups
            //var childGroups = _groupRepository.GetChildrenGroups(groupName);
            var childGroups = allInventoryGroups.Single(s => s.AnsibleGroupName == groupName.ToLower()).FlattenChildrends();
            var allGroupNames = childGroups.Select(g => g.Name).ToArray();

            // Find all servers for these groups
            var servers = await _serverRepository.ListAsync(new ServerSpecification(allGroupNames, environment));

            //Add custom groups based on ansible logic
            var customGroups = new List<Group>();
            customGroups.Add(new Group("all"));


            var serversDtoTasks = new List<Task<ServerDto>>();
            foreach (Server server in servers)
            {
                var serverDto = GetOrFillServerData(server);
                serversDtoTasks.Add(serverDto);
            }

            var results = await Task.WhenAll<ServerDto>(serversDtoTasks);

            var serverGroups = new List<Group>();
            var serversDto = new List<ServerDto>();
            foreach (ServerDto serverDto in results)
            {
                serversDto.Add(serverDto);
                //serverGroups.AddRange(server.GetInternalServerGroups());

                foreach (Group group in serverDto.Groups)
                {
                    var refGroup = allInventoryGroups.First(g => g.GroupId == group.GroupId);
                    serverGroups.Add(refGroup);
                    serverGroups.AddRange(refGroup.TraverseParents());
                    //dtoServer.Groups.AddRange(refGroup.TraverseParents());
                    //dtoServer.Groups.Add(refGroup);

                }
            }

           var allGroups = customGroups.Concat(serverGroups).Distinct();

            var inventory = new InventoryDto()
            {
                Servers = serversDto,
                Groups = allGroups.ToList()
            };

            return inventory;

        }

        public async Task<ServerDto> GetOrFillServerData(Server server)
        {
            var result = await UseCacheAsync<ServerDto>(async () =>
            {
                return await this.PopulateServerDtoData(server);
            }, $"server_{server.HostName}");

            return result;
        }

        private async Task<ServerDto> PopulateServerDtoData(Server server)
        {
            // Get AllGroups
            var allInventoryGroups = await this.GetAllGroupAsync();

            var dtoServer = new ServerDto();
            dtoServer.HostName = server.HostName;
            dtoServer.Variables = server.GetAnsibleVariables();

            // Read groups
            foreach (ServerGroup group in server.ServerGroups)
            {
                dtoServer.Groups.Add(group.Group);
            }

            return dtoServer;
        }













    }
}
