using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.API.Models
{
    public class ConfigurationProperty<T>
    {
        public String Name { get; set; }
        public T Value { get; set; }

    }
}
