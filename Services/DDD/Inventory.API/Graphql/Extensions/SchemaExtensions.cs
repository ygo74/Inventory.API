using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Inventory.API.Graphql.Extensions
{
    public static class SchemaExtensions
    {
        public static ISchema AddQuery<T>(this Schema schema, IServiceProvider provider) where T : IComplexGraphType
        {
            if (schema.Query == null)
            {
                var type = new ObjectGraphType
                {
                    Name = "query"
                };

                schema.Query = type;
            }

            //var fields =  schema.DependencyResolver.Resolve<T>().Fields;
            var fields =  provider.GetRequiredService<T>().Fields;

            foreach (var field in fields) schema.Query.AddField(field);

            return schema;
        }

        public static ISchema AddMutation<T>(this Schema schema, IServiceProvider provider) where T : IComplexGraphType
        {
            if (schema.Mutation == null)
            {
                var type = new ObjectGraphType
                {
                    Name = "mutation"
                };

                schema.Mutation = type;
            }

            //var fields = schema.DependencyResolver.Resolve<T>().Fields;
            var fields = provider.GetRequiredService<T>().Fields;

            foreach (var field in fields) schema.Mutation.AddField(field);

            return schema;
        }

    }
}
