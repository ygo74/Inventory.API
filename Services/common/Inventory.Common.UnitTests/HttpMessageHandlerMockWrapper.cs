using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.UnitTests.Moq.Http
{
    internal sealed class HttpMessageHandlerMockWrapper
    {
        public HttpMessageHandlerMockWrapper(
            Type typedHttpClientType,
            HttpMessageHandler httpMessageHandlerMock)
        {
            TypedHttpClientType = typedHttpClientType;
            HttpMessageHandlerMock = httpMessageHandlerMock;
        }

        public Type TypedHttpClientType { get; }
        public HttpMessageHandler HttpMessageHandlerMock { get; }
    }

}
