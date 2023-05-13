using Inventory.Common.Application.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Dto
{
    public interface ICreateOrUpdateConfigurationEntityRequest
    {
        bool? Deprecated { get; set; }

        DateTime? ValidFrom { get; set; }
        DateTime? ValidTo { get; set; }

        string InventoryCode { get; set; }
    }


    public abstract class CreateConfigurationEntityRequest<T> : IRequest<Payload<T>>, ICreateOrUpdateConfigurationEntityRequest where T : class
    {
        public bool? Deprecated { get; set; } = false;

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public string InventoryCode { get; set; }
    }

    public abstract class UpdateConfigurationEntityRequest<T> : IRequest<Payload<T>>, ICreateOrUpdateConfigurationEntityRequest where T : class
    {
        public int Id { get; set; }
        public bool? Deprecated { get; set; } = false;

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

        public string InventoryCode { get; set; }

    }

    public abstract class DeleteConfigurationEntityRequest<T> : IRequest<Payload<T>> where T : class
    {
        public int Id { get; set; }
    }

}
