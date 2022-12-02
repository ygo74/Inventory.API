using Inventory.Common.Infrastructure.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Datacenter
{
    public class DatacenterIntegrationEvent : IntegrationEvent
    {
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        public string Name { get; set; }
        public string InventoryCode { get; set; }
        public bool Deprecated { get; set; }
        public bool IsValid { get; set; }

    }
}
