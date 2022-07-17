using System;
using System.Collections.Generic;
using System.Text;

using MediatR;

namespace Inventory.Domain.Events
{
    public class GroupChangedEvent : IInternalNotification
    {
        public int GroupId { get; }
        public string test { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public GroupChangedEvent(int groupId) => GroupId = groupId;

    }
}
