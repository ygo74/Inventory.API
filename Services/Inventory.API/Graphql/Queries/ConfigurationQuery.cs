using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Inventory.API.Application.Dto;
using Inventory.API.Graphql.Types;
using Inventory.API.Infrastructure;
using Inventory.Domain.Models;
using Inventory.Domain.Models.Configuration;
using Inventory.Domain.Repositories.Interfaces;
using Inventory.Domain.Specifications;
using System.Collections.Generic;

namespace Inventory.API.Graphql.Queries
{
    public class ConfigurationQuery : ObjectGraphType
    {

        public ConfigurationQuery(IDataLoaderContextAccessor accessor, IAsyncRepository<Location> locationRepository,
                                                                       IAsyncRepository<TrustLevel> trustLevelRepository,
                                                                       IAsyncRepository<Inventory.Domain.Models.Configuration.Environment> environmentRepository,
                                                                       IAsyncRepository<Inventory.Domain.Models.Configuration.Application> applicationRepository)
        {

            Field<ListGraphType<LocationType>, List<Location>>()
                    .Name("Locations")
                    .ResolveAsync(ctx =>
                    {
                        return locationRepository.ListAsync(); 
                    });

            Field<ListGraphType<TrustLevelType>, List<TrustLevel>>()
                    .Name("TrustLevels")
                    .ResolveAsync(ctx =>
                    {
                        return trustLevelRepository.ListAsync();
                    });

            //Field<ListGraphType<EnvironmentType>, List<Inventory.Domain.Models.Environment>>()
            //        .Name("Environments")
            //        .Argument<StringGraphType>("name")
            //        .ResolveAsync(ctx =>
            //        {
            //            var envName = ctx.GetArgument<string>("name");
            //            if (string.IsNullOrWhiteSpace(envName))
            //            {
            //                var envSpec = new EnvironmentSpecification();
            //                return environmentRepository.ListAsync(envSpec);
            //            }
            //            else
            //            {
            //                var envSpec = new EnvironmentSpecification(envName);
            //                return environmentRepository.ListAsync(envSpec);
            //            }
            //        });

            Field<ListGraphType<ApplicationType>, List<Inventory.Domain.Models.Configuration.Application>>()
                    .Name("Applications")
                    .Argument<StringGraphType>("name")
                    .Argument<StringGraphType>("code")
                    .ResolveAsync(ctx =>
                    {
                        var appSpec = new ApplicationSpecification();
                        appSpec.Name = ctx.GetArgument<string>("name");
                        appSpec.Code = ctx.GetArgument<string>("code");
                        return applicationRepository.ListAsync(appSpec);
                        //return applicationRepository.ListAllAsync();

                    });

        }
    }
}
