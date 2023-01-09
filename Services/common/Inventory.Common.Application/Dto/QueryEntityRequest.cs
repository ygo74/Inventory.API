using Ardalis.Specification;
using Inventory.Common.Application.Core;
using Inventory.Common.Domain.Filters;
using Inventory.Common.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Dto
{

    public class QueryEntityOffsetPaginationRequest<TDto> : IRequest<OffsetPaginationPayload<TDto>> 
        where TDto : class
    {
        private OffsetPaginationRequest _pagination = new OffsetPaginationRequest();

        public OffsetPaginationRequest Pagination
        {
            get { return _pagination; }
            set { _pagination = value; }
        }
    }

    public class QueryEntityCursorPaginationRequest<TDto> : IRequest<CursorPaginationdPayload<TDto>> 
        where TDto : class

    {
        private CursorPaginationRequest _pagination = new CursorPaginationRequest();

        public CursorPaginationRequest Pagination
        {
            get { return _pagination; }
            set { _pagination = value; }
        }
    }
}
