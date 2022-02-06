using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.Network
{
    public class AzResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Values { get; set; }

    }
}