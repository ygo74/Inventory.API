using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Application.Dto;
using Inventory.API.Graphql.Types;
using Inventory.API.Infrastructure;
using Inventory.Domain.Models;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using System.Collections.Generic;

namespace Inventory.API.Graphql.Queries
{
    public class ConfigurationQuery : ObjectGraphType
    {

        public ConfigurationQuery(IDataLoaderContextAccessor accessor, IAsyncRepository<Location> locationRepository,
                                                                       IAsyncRepository<TrustLevel> trustLevelRepository,
                                                                       IAsyncRepository<Inventory.Domain.Models.Environment> environmentRepository)
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

            Field<ListGraphType<EnvironmentType>, IReadOnlyList<Inventory.Domain.Models.Environment>>()
                    .Name("Environments")
                    .Argument<StringGraphType>("name")
                    .ResolveAsync(ctx =>
                    {
                        var envName = ctx.GetArgument<string>("name");
                        if (string.IsNullOrWhiteSpace(envName))
                        {
                            var envSpec = new EnvironmentSpecification();
                            return environmentRepository.ListAsync(envSpec);
                        }
                        else
                        {
                            var envSpec = new EnvironmentSpecification(envName);
                            return environmentRepository.ListAsync(envSpec);
                        }
                    });

        }
    }
}
