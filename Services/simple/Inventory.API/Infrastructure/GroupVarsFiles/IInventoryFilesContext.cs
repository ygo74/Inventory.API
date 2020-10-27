using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Inventory.Infrastructure.GroupVarsFiles
{
    public interface IInventoryFilesContext
    {
        Dictionary<string, JObject> GetVariables(string inventoryPath);
        Task<Dictionary<string, JObject>> GetVariablesAsync();
    }
}