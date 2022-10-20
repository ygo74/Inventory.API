using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Api.Base.Dto
{
    public interface ICreateConfigurationEntityDto
    {
        bool? Deprecated { get; set; }

        DateTime? ValidFrom { get; set; }
        DateTime? ValidTo { get; set; }

    }


    public abstract class CreateConfigurationEntityDto<T> : IRequest<T>, ICreateConfigurationEntityDto
    {
        public bool? Deprecated { get; set; } = false;

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

    }
}
