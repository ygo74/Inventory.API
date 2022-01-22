using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Events
{
    public class ServerChangedEvent : INotification
    {

        public int ServerId { get; private set; }
        public string HostName { get; private set; }

        public ServerChangedEvent(int serverId, string hostName)
        {
            ServerId = serverId;
            HostName = hostName;
        }
    }
}
