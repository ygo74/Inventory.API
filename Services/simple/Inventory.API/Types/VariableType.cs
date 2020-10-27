using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using Inventory.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Types
{
    public class VariableType: ScalarGraphType
    {
        public VariableType()
        {
            Name = "Variable";
        }

        public override object ParseLiteral(IValue value)
        {
            if (value is VariableValue variableValue)
                return ParseValue(variableValue.Value);

            return value is StringValue stringValue
                ? ParseValue(stringValue.Value)
                : null;
        }

        public override object ParseValue(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Domain.Models.Variable));
        }

        public override object Serialize(object value)
        {
            return ValueConverter.ConvertTo(value, typeof(Domain.Models.Variable));
        }
    }
}
