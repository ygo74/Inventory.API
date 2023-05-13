using Ardalis.GuardClauses;
using AutoMapper;
using Inventory.Networks.Domain.Models;
using Inventory.Plugins.Azure.Models;
using Inventory.Plugins.Interfaces;
using Microsoft.Azure.Management.ResourceGraph;
using Microsoft.Azure.Management.ResourceGraph.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Plugins.Azure.Services
{
    public class AzSubnetProvider : ISubnetProvider
    {
        private readonly ILogger<AzSubnetProvider> _logger;
        private readonly IMapper _mapper;
        private AzCredential _credential;


        public string Name { get => "Azure Subnet Provider"; }

        public string Description { get => "Get list of subnets defined in microsoft.network/virtualnetworks of Azure Subscription"; }

        public AzSubnetProvider(ILogger<AzSubnetProvider> logger, IMapper mapper)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public void SetCredential(AzCredential credential)
        {
            _credential = Guard.Against.Null(credential, nameof(credential));
        }

        public async Task<List<Subnet>> ListAllAsync()
        {
            // string strQuery = "Resources | project name, type | limit 5";
            // string strQuery = "Resources | where type =~ 'microsoft.network/virtualnetworks'";
            var strQuery = new StringBuilder();
            strQuery.Append("Resources |");
            strQuery.Append(" where type =~ \"microsoft.network/virtualnetworks\" |");
            strQuery.Append(" extend subnet = properties.subnets |");
            strQuery.Append(" mv-expand subnet |");
            strQuery.Append(" project VNetName = name, location,resourceGroup, VNetCIDR = properties.addressSpace.addressPrefixes, subnetName = subnet.name, subnetCIDR = subnet.properties.addressPrefix, subnetId = subnet.id");

            AuthenticationContext authContext = new AuthenticationContext($"https://login.microsoftonline.com/{_credential.TenantId}");
            var clientCredential = new ClientCredential(_credential.ClientId.ToString(), _credential.Password);
            AuthenticationResult authResult = await authContext.AcquireTokenAsync("https://management.core.windows.net", clientCredential);
            ServiceClientCredentials serviceClientCreds = new TokenCredentials(authResult.AccessToken);

            ResourceGraphClient argClient = new ResourceGraphClient(serviceClientCreds);
            QueryRequest request = new QueryRequest();
            request.Query = strQuery.ToString();
            request.Options = new QueryRequestOptions { ResultFormat = ResultFormat.ObjectArray };

            QueryResponse response = argClient.Resources(request);

            //JArray allVmData = new JArray();
            //QueryResponse response;
            //do
            //{
            //    response = await argClient.ResourcesAsync(request);
            //    request.Options.SkipToken = response.SkipToken;

            //    allVmData.Merge(response.Data);
            //} while (!string.IsNullOrEmpty(response.SkipToken));

            //var test = JsonConvert.DeserializeObject<List<AsResourceInformation>>(allVmData.ToString());
            var allSubnets = System.Text.Json.JsonSerializer.Deserialize<List<AzVirtualNetwork>>(response.Data.ToString());

            var results = new List<Subnet>();

            foreach (var subnetAzure in allSubnets)
            {
                var subnet = _mapper.Map<Subnet>(subnetAzure);
                results.Add(subnet);
            }

            return results;
        }

        private async Task<string> GetResults(string token)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://management.azure.com/subscriptions/")
            };

            var SubscriptionGUID = _credential.SubscriptionId;

            string URI = $"{SubscriptionGUID}/providers/Microsoft.Network/virtualNetworks?api-version=2021-03-01";

            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await httpClient.GetAsync(URI);

            response.EnsureSuccessStatusCode();

            //var contentStream = await response.Content.ReadAsStreamAsync();
            var jsonString = await response.Content.ReadAsStringAsync();
            try
            {
                //var JSONObject = await JsonSerializer.DeserializeAsync<List<AzVirtualNetwork>>(contentStream, new System.Text.Json.JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                var JSONObject = System.Text.Json.JsonSerializer.Deserialize<AzResponse<AzVirtualNetwork>>(jsonString);
                //var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(JSONObject);
                return response.StatusCode.ToString();

            }
            catch (System.Text.Json.JsonException) // Invalid JSON
            {
                Console.WriteLine("Invalid JSON.");
                throw;
            }



        }


    }
}
