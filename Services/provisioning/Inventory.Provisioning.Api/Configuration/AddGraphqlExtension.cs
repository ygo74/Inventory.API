using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Extensions;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Inventory.Common.Application.Errors;
using Inventory.Common.Application.Graphql;
using Inventory.Common.Application.Users;
using Inventory.Provisioning.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Inventory.Common.Application.Graphql.Types.ErrorTypes;

namespace Inventory.Configuration.Api.Configuration
{
    public static class AddGraphqlExtension
    {
        public static IServiceCollection AddGraphqlServices(
            this IServiceCollection serviceCollection,
            IWebHostEnvironment env)
        {

            serviceCollection.AddGraphQLServer()
                .AddDiagnosticEventListener<ErrorLoggingDiagnosticsEventListener>()
                .RegisterDbContext<ProvisioningDbContext>(DbContextKind.Pooled)
                .SetPagingOptions(
                    new PagingOptions
                    {
                        IncludeTotalCount = true,
                        MaxPageSize = 200
                    }
                )
                .ModifyRequestOptions(requestExecutorOptions =>
                {
                    requestExecutorOptions.IncludeExceptionDetails = !env.IsProduction();
                })
                .AllowIntrospection(env.IsDevelopment())
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
                .AddProjections()
                .AddQueryType(d => d.Name("Query"))
                    //.AddTypeExtension<DatacenterQueries>()
                    //.AddTypeExtension<PluginQueries>()
                    //.AddTypeExtension<LocationQueries>()
                    //.AddTypeExtension<CredentialQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    //.AddTypeExtension<DatacenterMutations>()
                    //.AddTypeExtension<PluginMutations>()
                    //.AddTypeExtension<LocationMutations>()
                .AddType<GenericApiError>()
                .AddType<ValidationError>()
                .AddType<UnAuthorisedError>()
                .AddType<NotFoundError>()
                .AddType<BaseErrorInterfaceType>()
                // Locations
                //.AddType<GetLocationRequestType>()
                // Datacenters
                //.AddType<DatacenterType>()
                //.AddType<CreateDatacenterInputType>()
                //.AddType<CreateDatacenterPayloadType>()
                // Plugins
                //.AddType<PluginType>()
                //.AddType<CreatePluginInputType>()
                // Credentials
                //.AddType<CredentialType>()

                //.AddMutationConventions(
                //    new MutationConventionOptions
                //    {
                //        InputArgumentName = "input",
                //        InputTypeNamePattern = "{MutationName}Input",
                //        PayloadTypeNamePattern = "{MutationName}Payload",
                //        PayloadErrorTypeNamePattern = "{MutationName}Error",
                //        PayloadErrorsFieldName = "errors",
                //        ApplyToAllMutations = true
                //    })
                .BindRuntimeType<DateTime, DateTimeType>()
                .BindRuntimeType<int, IntType>()
                .BindRuntimeType<long, LongType>()
                //.BindRuntimeType<Dictionary<string, bool>, AnyType>()
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
