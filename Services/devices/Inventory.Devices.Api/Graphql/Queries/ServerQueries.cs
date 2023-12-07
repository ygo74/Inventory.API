using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Inventory.Common.Domain.Repository;
using Inventory.Devices.Api.Applications.Servers;
using Inventory.Devices.Domain.Models;
using Inventory.Devices.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Devices.Api.Graphql.Queries
{
    [ExtendObjectType(OperationTypeNames.Query)]
    public class ServerQueries
    {
        /// <summary>
        /// Return server current date-time
        /// </summary>
        /// <returns>DateTime current date time</returns>
        //public DateTime GetServerDateTime() => DateTime.Now;

        public string GetStatus() => "OK";

        [UseFiltering]
        public Task<List<Server>> GetServers([Service]IAsyncRepositoryWithSpecification<Server> _repository, IResolverContext ctx)
        {
            return _repository.ListAsync();
        }

        [UsePaging]
        [UseSorting]
        [UseFiltering]
        public IQueryable<Device> GetServers2([Service] IDbContextFactory<ServerDbContext> dbContextFactory, IResolverContext ctx)
        {
            var dbContext = dbContextFactory.CreateDbContext();
            //return dbContext.Devices;
            return null;
        }

        public async Task<IReadOnlyList<ServerDto>> GetSummaryServers([Service] IMediator mediator, IResolverContext ctx, string? datacenterName)
        {

            // Request
            var request = new GetServerSummaryRequest()
            {
                DatacenterName = datacenterName
            };

            var result = await mediator.Send(request);

            return result.Data;
        }



    }
}
