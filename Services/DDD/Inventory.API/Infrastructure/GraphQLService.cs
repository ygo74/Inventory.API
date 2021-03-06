﻿using AutoMapper;
using Inventory.API.Application.Dto;
using Inventory.API.Graphql.Filters;
using Inventory.Domain;
using Inventory.Domain.Extensions;
using Inventory.Domain.Filters;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using Microsoft.AspNetCore.Http;
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
        private readonly IAsyncRepository<Inventory.Domain.Models.Application> _applicationRepository;
        private readonly IMemoryCache _cache;

        private readonly IMapper _mapper;

        private readonly IHttpContextAccessor _httpContextAccessor;



        public GraphQLService(InventoryService inventoryService, IGroupRepository groupRepository, IAsyncRepository<Server> serverRepository,
                                IAsyncRepository<Inventory.Domain.Models.Application> applicationRepository,
                                IMemoryCache cache, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _inventoryService = inventoryService ?? throw new ArgumentNullException(nameof(inventoryService));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        }


        #region "Servers"
        public async Task<ILookup<int, ServerDto>> GetServersByGroupAsync(IEnumerable<int> groupIds, CancellationToken token)
        {
            var serverGroups = await _inventoryService.GetServersByGroupAsync(groupIds);
            return serverGroups.ToLookup(s => s.GroupId, s =>
            {
                var dtoServer = GetOrFillServerData(s.Server);
                dtoServer.Wait();
                return dtoServer.Result;
            });

        }

        public async Task<ILookup<int, ServerDto>> GetServersByEnvironmentAsync(IEnumerable<int> environmentIds, CancellationToken token)
        {
            var serverFilter = new ServerFilter()
            {
                EnvironmentIds = environmentIds.ToArray()
            };

            var serverGroups = await _serverRepository.ListAsync(new ServerSpecification(serverFilter));
            var groupBy = serverGroups.SelectMany(s => s.ServerEnvironments, (s, se) => new { se.EnvironmentId, se.Server });

            return groupBy.ToLookup(s => s.EnvironmentId, s =>
            {
                var dtoServer = GetOrFillServerData(s.Server);
                dtoServer.Wait();
                return dtoServer.Result;
            });

        }

        public async Task<ILookup<int, ServerDto>> GetServersByApplicationAsync(IEnumerable<int> applicationIds, CancellationToken token)
        {
            var appSpec = new ApplicationSpecification()
            {
                ApplicationIds = applicationIds.ToArray()
            };

            var applicationsServers = await _applicationRepository.ListAsync(appSpec);
            return applicationsServers.SelectMany(a => a.Servers, (a, srv) => new { a.ApplicationId, srv }).ToLookup(a => a.ApplicationId, a =>
            {
                var dtoServer = GetOrFillServerData(a.srv);
                dtoServer.Wait();
                return dtoServer.Result;
            });

        }


        public async Task<IReadOnlyList<ServerDto>> GetAllServersAsync(ServerFilter filter)
        {
            var serverSpec = new ServerSpecification(filter);
            var servers = await _serverRepository.ListAsync(serverSpec);

            // Load Server Additional Data and calculate list of groups in this inventory
            var serversDto = new System.Collections.Concurrent.ConcurrentBag<ServerDto>();

            Parallel.ForEach<Server>(servers, async currentServer =>
            {
                var serverDto = await GetOrFillServerData(currentServer);
                serversDto.Add(serverDto);
            });

            return serversDto.ToList();

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

            var claims = _httpContextAccessor.HttpContext.User.Identities.FirstOrDefault().Claims.ToList();


            // Get AllGroups
            var allInventoryGroups = await this.GetAllGroupAsync();

            // Find all Allowed groups and child groups
            //var childGroups = _groupRepository.GetChildrenGroups(groupName);
            var childGroups = allInventoryGroups.Single(s => s.AnsibleGroupName == groupName.ToLower()).FlattenChildrends();
            var allGroupNames = new List<string>() { groupName }
                                    .Concat(
                                        childGroups.Select(g => g.Name)
                                    ).ToArray();

            // Find all servers for these groups
            var servers = await _serverRepository.ListAsync(new ServerSpecification(allGroupNames, environment));

            //Add custom groups based on ansible logic
            var customGroups = new List<Group>();
            customGroups.Add(new Group("all"));

            // Load Server Additional Data and calculate list of groups in this inventory
            var serversDto = new System.Collections.Concurrent.ConcurrentBag<ServerDto>();
            var serverGroups = new System.Collections.Concurrent.ConcurrentDictionary<String, Group>();

            Parallel.ForEach<Server>(servers, async currentServer =>
            {
                var serverDto = await GetOrFillServerData(currentServer);
                serversDto.Add(serverDto);
                foreach (Group group in serverDto.Groups)
                {

                    if (group.GroupId <= 0)
                    {
                        serverGroups.GetOrAdd(group.Name, group);
                    }
                    else
                    {
                        var refGroup = allInventoryGroups.First(g => g.GroupId == group.GroupId);
                        serverGroups.GetOrAdd(refGroup.Name, refGroup);
                        foreach (Group parentGroup in refGroup.TraverseParents())
                        {
                            serverGroups.GetOrAdd(parentGroup.Name, parentGroup);
                        }
                    }
                }
            });


            var allGroups = customGroups.Concat(serverGroups.Values).Distinct();

            var inventory = new InventoryDto()
            {
                Servers = serversDto.ToList(),
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
            var dtoServer = _mapper.Map<ServerDto>(server);
            dtoServer.Variables = server.GetAnsibleVariables();

            // Add custom Information
            // Groups
            if (server.Status == ServerStatus.To_be_Created)
            {
                dtoServer.Groups.Add(new Group("New"));
            }

            return dtoServer;
        }













    }
}
