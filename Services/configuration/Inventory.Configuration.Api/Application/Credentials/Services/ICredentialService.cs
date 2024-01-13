using System.Threading.Tasks;
using System.Threading;

namespace Inventory.Configuration.Api.Application.Credentials.Services
{
    public interface ICredentialService
    {
        Task<bool> CredentialExists(int id, CancellationToken cancellationToken);
        Task<bool> CredentialExists(string name, CancellationToken cancellationToken = default);

    }
}
