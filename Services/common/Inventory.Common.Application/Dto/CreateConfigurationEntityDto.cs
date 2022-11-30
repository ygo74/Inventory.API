using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Dto
{
    public interface ICreateOrUpdateConfigurationEntityDto
    {
        bool? Deprecated { get; set; }

        DateTime? ValidFrom { get; set; }
        DateTime? ValidTo { get; set; }

    }


    public abstract class CreateConfigurationEntityDto<T> : IRequest<T>, ICreateOrUpdateConfigurationEntityDto
    {
        public bool? Deprecated { get; set; } = false;

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

    }

    public abstract class UpdateConfigurationEntityDto<T> : IRequest<T>, ICreateOrUpdateConfigurationEntityDto
    {
        public int Id { get; set; }
        public bool? Deprecated { get; set; } = false;

        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

    }

}
