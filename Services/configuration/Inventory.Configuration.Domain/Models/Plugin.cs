using Ardalis.GuardClauses;
using Inventory.Common.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Configuration.Domain.Models
{
    public class Plugin : ConfigurationEntity
    {
        public string Name { get; private set; }
        public string Code { get; private set; }
        public string Version { get; private set; }


        public string Path { get; private set; }

        protected Plugin() { }

        public Plugin(string name, string code, string version, bool? deprecated = null, DateTime? startDate = null, DateTime? endDate = null) 
            : base(deprecated, startDate, endDate)
        {
            Name = Guard.Against.NullOrWhiteSpace(name, nameof(name));
            Code = Guard.Against.NullOrWhiteSpace(code, nameof(code));
            Version = Guard.Against.NullOrWhiteSpace(version, nameof(version));
        }

        public void SetPath(string path)
        {
            Path = Guard.Against.NullOrWhiteSpace(path, nameof(path));
        }
    }
}
