using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Application.Dto;
using Inventory.API.Graphql.Types;
using Inventory.API.Infrastructure;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using System.Collections.Generic;

namespace Inventory.API.Graphql.Queries
{
    public class ConfigurationQuery : ObjectGraphType
    {

        public ConfigurationQuery(IDataLoaderContextAccessor accessor, IAsyncRepository<Location> locationRepository, IAsyncRepository<TrustLevel> trustLevelRepository)
        {

            Field<ListGraphType<LocationType>, IReadOnlyList<Location>>()
                    .Name("Locations")
                    .ResolveAsync(ctx =>
                    {
                        return locationRepository.ListAllAsync(); 
                    });

            Field<ListGraphType<TrustLevelType>, IReadOnlyList<TrustLevel>>()
                    .Name("TrustLevels")
                    .ResolveAsync(ctx =>
                    {
                        return trustLevelRepository.ListAllAsync();
                    });

        }
    }
}
