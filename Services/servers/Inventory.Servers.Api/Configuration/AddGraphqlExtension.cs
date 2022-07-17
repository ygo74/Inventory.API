using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using HotChocolate.Execution.Configuration;
using HotChocolate.AspNetCore.Extensions;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Inventory.Servers.Api.Graphql.Queries;
using Inventory.Servers.Api.Graphql.Mutations;
using static Inventory.Servers.Api.Graphql.Types.ErrorTypes;
using Inventory.Servers.Infrastructure;
using HotChocolate.Data;

namespace Inventory.Servers.Api.Configuration
{
    public static class AddGraphqlExtension
    {
        public static IServiceCollection AddGraphqlServices(
            this IServiceCollection serviceCollection,
            IWebHostEnvironment env)
        {

            serviceCollection.AddGraphQLServer()
                .RegisterDbContext<ServerDbContext>(DbContextKind.Pooled)
                //.SetPagingOptions(
                //    new PagingOptions
                //    {
                //        IncludeTotalCount = true,
                //        MaxPageSize = 200
                //    }
                //)
                //.ModifyRequestOptions(requestExecutorOptions =>
                //{
                //    requestExecutorOptions.IncludeExceptionDetails = !env.IsProduction();
                //})
                //.AllowIntrospection(env.IsDevelopment())
                //.AddExportDirectiveType()

                //.ModifyOptions(options =>
                //{
                //    options.UseXmlDocumentation = true;

                //    options.SortFieldsByName = true;

                //    options.RemoveUnreachableTypes = true;
                //})

                //.AddGlobalObjectIdentification()
                //.AddQueryFieldToMutationPayloads()

                //.AddHttpRequestInterceptor<IntrospectionInterceptor>()
                //// This 2 are temporary workerounds to enable stream under relay
                //.TryAddTypeInterceptor<StreamTypeInterceptor>()
                //.AddHttpRequestInterceptor<StreamRequestInterceptor>()

                //.AddInstrumentation(opt =>
                //{
                //    opt.RenameRootActivity = true;
                //    opt.RequestDetails = RequestDetails.All;
                //})

                .AddFiltering()
                .AddSorting()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<ServerQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<ServerMutations>()

                .BindRuntimeType<DateTime, DateTimeType>()
                .BindRuntimeType<int, IntType>()
                .BindRuntimeType<long, LongType>()
                .AddType<BaseErrorType>()
                .AddType<BaseErrorInterfaceType>()
                .AddType<ValidationErrorType>()
                .AddType<UnAuthorisedErrorType>()
                //.AddType<CreateWebHookErrorUnion>()
                ;

            return serviceCollection;
        }

        public static GraphQLEndpointConventionBuilder MapGraphQLEndpoint(
                   this IEndpointRouteBuilder builder)
        {
            var env = builder.ServiceProvider.GetService<IWebHostEnvironment>();

            return builder.MapGraphQL()
            .WithOptions(new GraphQLServerOptions
            {

                EnableSchemaRequests = env.IsDevelopment(),
                Tool = {
                    Enable = env.IsDevelopment(),
                }
            });
        }

    }

}
