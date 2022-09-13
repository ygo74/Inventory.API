using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Base.Models
{
    public class Event : INotification
    {
        public enum EntityAction
        {
            Added,
            Updated,
            Removed
        }

        public EntityAction Action { get; private set; }

        public Event(EntityAction action)
        {
            Action = action;

        }
    }

    public class Event<T> : Event
    {

        public T Data { get; private set; }


        public Event(T data, EntityAction action) : base(action)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

    }

}
