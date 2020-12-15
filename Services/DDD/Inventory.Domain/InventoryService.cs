using Inventory.Domain.Models;
using Inventory.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;

using MediatR;
using Inventory.Domain.Events;

namespace Inventory.Domain
{
    public class InventoryService : IInventoryService
    {

        private readonly IAsyncRepository<Server> _serverRepository;
        private readonly IAsyncRepository<Group> _groupRepository;
        private readonly IAsyncRepository<Models.OperatingSystem> _osRepository;
        private readonly IAsyncRepository<Models.Environment> _envRepository;
        private readonly IAsyncRepository<ServerGroup> _serverGroupRepository;

        private readonly IMediator _mediator;

        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IAsyncRepository<Server> serverRepository,
                                IAsyncRepository<Group> groupRepository,
                                IAsyncRepository<Models.OperatingSystem> osRepository,
                                IAsyncRepository<Models.Environment> envRepository,
                                IAsyncRepository<ServerGroup> serverGroupRepository,
                                ILogger<InventoryService> logger,
                                IMediator mediator)
        {
            _serverRepository = serverRepository ?? throw new ArgumentNullException(nameof(serverRepository));
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
            _osRepository = osRepository ?? throw new ArgumentNullException(nameof(osRepository));
            _envRepository = envRepository ?? throw new ArgumentNullException(nameof(envRepository));
            _serverGroupRepository = serverGroupRepository ?? throw new ArgumentNullException(nameof(serverGroupRepository));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        #region Operating Systems
        public async Task<Models.OperatingSystem> GetorAddOperatingSystemByNameAsync(OsFamilly osFamilly, string name)
        {
            _logger.LogDebug($"Get Or Add Operating System for {osFamilly} by : '{name}'");
            var os = await _osRepository.FirstOrDefaultAsync(new OsSpecification(name));
            if (null == os)
            {
                _logger.LogDebug($"Create Operating System for {osFamilly} with name : '{name}'");
                os = new Models.OperatingSystem(name, osFamilly);
                await _osRepository.AddAsync(os);
                await this.GetorAddGroupAsync(name, osFamilly.ToString());
            }

            return os;
        }

        #endregion

        #region Environments

        #endregion


        #region Servers
        public Task<IReadOnlyList<ServerGroup>> GetServersByGroupAsync(IEnumerable<int> groupIds)
        {
            var serverGroups = _serverGroupRepository.ListAsync(new ServerByGroupsSpecification(groupIds));
            return serverGroups;
        }

        public Task<Server> GetServerByIdAsync(int id)
        {
            _logger.LogDebug($"Get Server by id : '{id}'");
            var server = _serverRepository.FirstAsync(new ServerSpecification(id));

            return server;
        }

        public async Task<Server> AddServerAsync(string hostName, OsFamilly osFamilly, string operatingSystemName, string environmentName, System.Net.IPAddress subnetIP)
        {

            var os = await this.GetorAddOperatingSystemByNameAsync(osFamilly, operatingSystemName);
            var env = await _envRepository.FirstAsync(new EnvironmentSpecification(environmentName));

            var server = new Server(hostName, os, env, 2, 4, subnetIP);
            var groupOS = await _groupRepository.FirstAsync(new GroupSpecification(os.Name));

            groupOS.AddServer(server);
            await _groupRepository.UpdateAsync(groupOS);
            return server;
        }

        //public Task<IReadOnlyList<Group>> GetServerGroups(Server server)
        //{

        //}


        #endregion

        #region Groups
        public Task<Group> GetGroupByIdAsync(int id)
        {
            _logger.LogDebug($"Get Group by id : '{id}'");
            var group = _groupRepository.GetByIdAsync(id);

            return group;
        }

        public async Task<Group> GetorAddGroupAsync(string name, String parentName, string ansibleGroupName = null)
        {
            var group = await _groupRepository.FirstOrDefaultAsync(new GroupSpecification(name));
            if (null == group)
            {
                group = new Group(name, ansibleGroupName);

                if (!String.IsNullOrWhiteSpace(parentName))
                {
                    var parent = await _groupRepository.FirstAsync(new GroupSpecification(parentName));
                    parent.AddSubGroups(group);
                }
                
                await _groupRepository.AddAsync(group);
                await _mediator.Publish(new GroupChangedEvent(group.GroupId));
            }

            return group;
        }

        #endregion


    }
}
