using GraphQL;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Server.Transports.AspNetCore.Common;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Graphql.Middlewares
{
    public class DebugPipelineMiddleware<TSchema> : GraphQLHttpMiddleware<TSchema>
        where TSchema : ISchema
    {
        private readonly RequestDelegate _next;

        public DebugPipelineMiddleware(RequestDelegate next, PathString path, IGraphQLRequestDeserializer requestDeserializer) : base(next, path, requestDeserializer)
        {
            _next = next;
        }


    }
}
