using System.Threading;
using System.Threading.Tasks;

public interface ILocationService
{
    Task<bool> LocationExists(int id, CancellationToken cancellationToken);
    Task<bool> LocationExists(string countryCode, string cityCode, string regionCode, CancellationToken cancellationToken = default);
}
