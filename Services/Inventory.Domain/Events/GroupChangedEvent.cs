using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Inventory.Domain.Events
{
    public class GroupChangedEvent : INotification
    {
        public int GroupId { get; }

        public GroupChangedEvent(int groupId) => GroupId = groupId;

    }
}
