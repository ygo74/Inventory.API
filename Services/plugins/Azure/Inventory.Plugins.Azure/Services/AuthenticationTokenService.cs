using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace Inventory.Plugins.Azure.Services
{
    public class AuthenticationTokenService
    {
        private ILogger<AuthenticationTokenService> _logger;

        public AuthenticationTokenService(ILogger<AuthenticationTokenService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task<string> GetAccessToken(string tenantId, string clientId, string clientKey)
        {
            _logger.LogInformation($"Begin GetAccessToken for tenant {tenantId} and client {clientId}");

            string authContextURL = "https://login.windows.net/" + tenantId;
            var authenticationContext = new AuthenticationContext(authContextURL);
            var credential = new ClientCredential(clientId, clientKey);
            var result = await authenticationContext.AcquireTokenAsync("https://management.azure.com/", credential);

            if (result == null)
            {
                throw new InvalidOperationException("Failed to obtain the JWT token");
            }
            string token = result.AccessToken;
            return token;
        }

    }
}
