using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using Inventory.API.Types;
using Inventory.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API
{
    public class InventorySchema : Schema
    {
        public InventorySchema(IServiceProvider resolver) : base(resolver)
        {
            ValueConverter.Register<string, Domain.Models.Variable>(ParseVariable);
            RegisterValueConverter(new VariableAstValueConverter());

            Query = resolver.GetRequiredService<InventoryQuery>();
            Mutation = resolver.GetRequiredService<InventoryMutation>();
        }

        private Domain.Models.Variable ParseVariable(string arg)
        {
            return new NumericVariable(){ Name = "test", Value = 5};
        }
    }


    public class VariableValue : ValueNode<Domain.Models.Variable>
    {
        public VariableValue(Domain.Models.Variable value)
        {
            Value = value;
        }

        protected override bool Equals(ValueNode<Domain.Models.Variable> node)
        {
            return Value.Equals(node.Value);
        }
    }

    public class VariableAstValueConverter : IAstFromValueConverter
    {
        public IValue Convert(object value, IGraphType type)
        {
            return new VariableValue((Domain.Models.Variable)value);
        }

        public bool Matches(object value, IGraphType type)
        {
            return value is Domain.Models.Variable;
        }
    }
}
