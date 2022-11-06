using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inventory.Plugins.Azure.Models
{
    public class AzResponse<T>
    {
        [JsonPropertyName("value")]
        public List<T> Values { get; set; }

    }
}