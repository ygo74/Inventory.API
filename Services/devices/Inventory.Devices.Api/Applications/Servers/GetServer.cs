using Ardalis.GuardClauses;
using Inventory.Common.Application.Core;
using Inventory.Common.Application.Dto;
using Inventory.Common.Domain.Filters;
using Inventory.Devices.Domain.Interfaces;
using Inventory.Devices.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Devices.Domain.Filters;
using System.Linq;

namespace Inventory.Devices.Api.Applications.Servers
{
#nullable enable
    public class GetServerSummaryRequest : QueryEntityOffsetPaginationRequest<ServerDto>
    {
        public string? DatacenterName { get; set; }
    }
#nullable disable

    public class GetServerSummaryHandler : IRequestHandler<GetServerSummaryRequest, OffsetPaginationPayload<ServerDto>>
    {

        private readonly ILogger<GetServerSummaryHandler> _logger;
        private readonly IDeviceQueryStore _deviceQueryStore;

        public GetServerSummaryHandler(ILogger<GetServerSummaryHandler> logger,
                                       IDeviceQueryStore deviceQueryStore) 
        { 
            _logger = Guard.Against.Null(logger, nameof(logger));
            _deviceQueryStore = Guard.Against.Null(deviceQueryStore, nameof(deviceQueryStore));
        }

        public async Task<OffsetPaginationPayload<ServerDto>> Handle(GetServerSummaryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetServerSummaryHandler.Handle called");

            // filter
            var filter = ExpressionFilterFactory.Create<Server>()
                                                .WithDatacenter(request.DatacenterName);

            // execute query
            var result = await _deviceQueryStore.GetByCriteriaAsync<ServerDto>(filter);

            return new OffsetPaginationPayload<ServerDto>()
            {
                Data = result.ToList()
            };

        }
    }




}
