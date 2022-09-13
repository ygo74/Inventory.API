using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Configuration.Api.Application
{
    public class CreateConfigurationEntity<T> : IRequest<T>
    {
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

    }
}
