using Inventory.Common.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Provisioning.Domain.Models
{
    public class ConfigurationSpecification : AuditEntity
    {
        public string Kind { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Dictionary<string, Object> Specification { get; private set; } = new Dictionary<string, Object>();

        private List<Label> _labels = new List<Label>();
        public ICollection<Label> Labels => _labels.AsReadOnly();

    }
}
