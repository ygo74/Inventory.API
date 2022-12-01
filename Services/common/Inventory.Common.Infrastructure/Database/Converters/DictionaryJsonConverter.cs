using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inventory.Common.Infrastructure.Database.Converters
{
    public class DictionaryJsonConverter : ValueConverter<Dictionary<string, object>, string>
    {
        public DictionaryJsonConverter() 
            : base(
                    value => JsonSerializer.Serialize(value, typeof(Dictionary<string, object>), new System.Text.Json.JsonSerializerOptions()),
                    value => JsonSerializer.Deserialize<Dictionary<string, object>>(value, new System.Text.Json.JsonSerializerOptions())
                  )
        {
        }

    }
}
