using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Events
{
    public class ServerChangedEvent : IInternalNotification
    {

        public int ServerId { get; private set; }
        public string HostName { get; private set; }
        public string test { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ServerChangedEvent(int serverId, string hostName)
        {
            ServerId = serverId;
            HostName = hostName;
        }
    }
}
