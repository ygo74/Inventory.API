using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inventory.Domain.Events;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Inventory.API.Application.EventHandlers
{
    public class GroupEventChangedHandler //: INotificationHandler<GroupChangedEvent>
    {

        private readonly IMemoryCache _cache;

        public GroupEventChangedHandler(IMemoryCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));

        }

        public Task Handle(GroupChangedEvent notification, CancellationToken cancellationToken)
        {
            _cache.Remove("test");
            return Task.CompletedTask;
        }
    }
}
