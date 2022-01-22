using AutoMapper;
using Inventory.API.Application.Dto;
using Inventory.API.Infrastructure.Services;
using Inventory.Domain.Filters;
using Inventory.Domain.Models.ManagedEntities;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.API.Application.Servers
{
    public class ServerService
    {
        private readonly IAsyncRepository<Server> _serverRepository;
        private readonly IMapper _mapper;
        private readonly CachingService _cachingService;

        public ServerService(IAsyncRepository<Server> serverRepository, IMapper mapper, CachingService cachingService)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cachingService = cachingService ?? throw new ArgumentNullException(nameof(cachingService));
        }


        //public async Task<ILookup<int, ServerDto>> GetServersByApplicationAsync(IEnumerable<int> applicationIds, CancellationToken token)
        //{
        //    var appSpec = new ApplicationSpecification()
        //    {
        //        ApplicationIds = applicationIds.ToArray()
        //    };

        //    var applicationsServers = await _applicationRepository.ListAsync(appSpec);
        //    return applicationsServers.SelectMany(a => a.Servers, (a, srv) => new { a.ApplicationId, srv }).ToLookup(a => a.ApplicationId, a =>
        //    {
        //        var dtoServer = GetOrFillServerData(a.srv);
        //        dtoServer.Wait();
        //        return dtoServer.Result;
        //    });

        //}


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

        public async Task<ServerDto> GetOrFillServerData(Server server)
        {
            var result = await _cachingService.UseCacheAsync<ServerDto>(async () =>
            {
                return await this.PopulateServerDtoData(server);
            }, $"server_{server.HostName}");

            return result;
        }

        private async Task<ServerDto> PopulateServerDtoData(Server server)
        {
            var dtoServer = _mapper.Map<ServerDto>(server);
            //dtoServer.Variables = server.GetAnsibleVariables();

            // Add custom Information
            // Groups
            //if (server.Status == Domain.Enums.LifecycleStatus.New)
            //{
            //    dtoServer.Groups.Add(new Group("New"));
            //}

            return dtoServer;
        }


    }
}
