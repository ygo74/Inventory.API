using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Inventory.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class GraphQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDocumentWriter _writer;
        private readonly IDocumentExecuter _executor;

        public GraphQLMiddleware(RequestDelegate next, IDocumentWriter writer, IDocumentExecuter executor)
        {
            _next = next;
            _writer = writer;
            _executor = executor;
        }


        public async Task InvokeAsync(HttpContext context, ISchema schema, IServiceProvider serviceProvider)
        {
            if (context.Request.Path.StartsWithSegments("/graphql") && string.Equals(context.Request.Method, "POST", StringComparison.OrdinalIgnoreCase))
            {
                var request = await JsonSerializer.DeserializeAsync<GraphQLRequest>
                    (
                        context.Request.Body,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                    );

                var executer = new DocumentExecuter();
                var result = await executer.ExecuteAsync(options =>
                {
                    options.Schema = schema;
                    options.Query = request.Query;
                    options.OperationName = request.OperationName;
                    options.Inputs = request.Variables.ToInputs();

                    options.Listeners.Add(serviceProvider.GetRequiredService<DataLoaderDocumentListener>());

                }).ConfigureAwait(false);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 200; // OK

                var writer = new GraphQL.SystemTextJson.DocumentWriter();
                await writer.WriteAsync(context.Response.Body, result);

            }
            else
            {
                await _next(context);
            }
        }
    }

    public class GraphQLRequest
    {
        public string OperationName { get; set; }

        public string Query { get; set; }

        [JsonConverter(typeof(GraphQL.SystemTextJson.ObjectDictionaryConverter))]
        public Dictionary<string, object> Variables { get; set; }

    }


    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GraphQLMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphQLMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GraphQLMiddleware>();
        }
    }
}
