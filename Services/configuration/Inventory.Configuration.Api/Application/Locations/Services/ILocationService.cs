using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application.Locations.Services
{
    public interface ILocationService
    {
        Task<bool> LocationExists(int id, CancellationToken cancellationToken);
        Task<bool> LocationExists(string countryCode, string cityCode, string regionCode, CancellationToken cancellationToken = default);
    }
}
