using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using Microsoft.Rest;
using Microsoft.Azure.Management.ResourceGraph;
using Microsoft.Azure.Management.ResourceGraph.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace Inventory.Infrastructure.Network
{
    public class AzureProvider
    {

        private ILogger<AzureProvider> _logger;

        public AzureProvider(ILogger<AzureProvider> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



    }
}
