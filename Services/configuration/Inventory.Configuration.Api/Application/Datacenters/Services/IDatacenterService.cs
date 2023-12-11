using System.Threading.Tasks;
using System.Threading;

namespace Inventory.Configuration.Api.Application.Datacenters.Services
{
    public interface IDatacenterService
    {
        Task<bool> DatacenterExists(int id, CancellationToken cancellationToken);
        Task<bool> DatacenterExists(string code, CancellationToken cancellationToken = default);
    }
}
