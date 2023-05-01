using Microsoft.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.UnitTests.Moq.Http
{
    internal sealed class TestHttpMessageHandlerBuilderFilter
     : IHttpMessageHandlerBuilderFilter
    {
        private readonly IEnumerable<HttpMessageHandlerMockWrapper> _httpMessageHandlerWrappers;

        public TestHttpMessageHandlerBuilderFilter(
            // Injection of previously registered maps
            IEnumerable<HttpMessageHandlerMockWrapper> httpMessageHandlerWrappers)

        {
            _httpMessageHandlerWrappers = httpMessageHandlerWrappers;
        }

        public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
        {
            return builder =>
            {
                // Checking if a given HttpClient has a registered HttpMessageHandler mock
                var mockHandlerWrapper = _httpMessageHandlerWrappers
                    .SingleOrDefault(x =>
                        x.TypedHttpClientType.Name.Equals(
                            builder.Name,
                            StringComparison.InvariantCultureIgnoreCase));

                if (mockHandlerWrapper is not null)
                {
                    // If so, the default handler is replaced with mock
                    System.Diagnostics.Debug.WriteLine($"Overriding {nameof(builder.PrimaryHandler)} for '{builder.Name}' typed HTTP client");
                    builder.PrimaryHandler = mockHandlerWrapper.HttpMessageHandlerMock;
                }
                next(builder);
            };
        }
    }
}
