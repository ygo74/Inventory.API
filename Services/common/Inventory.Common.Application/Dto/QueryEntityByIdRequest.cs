using Inventory.Common.Application.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Dto
{
    public class QueryEntityByIdRequest<T> : IRequest<Payload<T>> where T : class
    {
        public int Id { get; set; }
    }
}
