using Inventory.Common.Application.Core;
using MR.AspNetCore.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Extensions
{
    public static class PagingExtensions
    {
        //public static CursorPaginationdPayload<T> GetPaginationPayload<T>(this KeysetPaginationResult<T> result, Expression<Func<string, string>> value)
        //{
        //    return new CursorPaginationdPayload<T>
        //    {
        //        TotalCount = result.TotalCount,
        //        Data = result.Data,
        //        HasNext = result.HasNext,
        //        HasPrevious = result.HasPrevious,
        //        StartCursor = result.Data.Count > 0 ? result.Data[0].Id.ToString() : string.Empty,
        //        EndCursor = result.Data.Count > 0 ? result.Data[result.Data.Count - 1].Id.ToString() : string.Empty,
        //    };

        //}
    }
}
