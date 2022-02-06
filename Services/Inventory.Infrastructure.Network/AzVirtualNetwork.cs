﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Network
{
    public class AzVirtualNetwork
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("VNetName")]
        public string Name { get; set; }

        [JsonPropertyName("VNetCIDR")]
        public string[] VNetCIDR { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("resourceGroup")]
        public string ResourceGroup { get; set; }

        [JsonPropertyName("subnetName")]
        public string SubnetName { get; set; }

        [JsonPropertyName("subnetCIDR")]
        public string SubnetCIDR { get; set; }

    }
}
