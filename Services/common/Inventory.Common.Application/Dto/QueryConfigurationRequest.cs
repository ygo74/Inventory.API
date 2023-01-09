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

    public class QueryConfigurationEntity<TDto> where TDto : ConfigurationEntityDto
    {
        public bool IncludeDeprecated { get; set; }
        public bool AllEntities { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }

    }

    public class QueryConfigurationCursorPaginationRequest<TDto> : QueryConfigurationEntity<TDto>, IRequest<CursorPaginationdPayload<TDto>>
        where TDto : ConfigurationEntityDto

    {
        private CursorPaginationRequest _pagination = new CursorPaginationRequest();

        public CursorPaginationRequest Pagination
        {
            get { return _pagination; }
            set { _pagination = value; }
        }
    }

    public static class QueryConfigurationEntityExtensions
    {

        public static IExpressionFilter<T> GetConfigurationEntityFilter<T, TDto>(this QueryConfigurationEntity<TDto> queryEntity)
            where T : ConfigurationEntity
            where TDto : ConfigurationEntityDto
        {
            var filter = ExpressionFilterFactory.Create<T>();
            if (!queryEntity.IncludeDeprecated) { filter = filter.ExcludeDeprecated(); }
            if (!queryEntity.AllEntities) { filter = filter.Valid(); }
            return filter;
        }

    }

}
    