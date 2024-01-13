using HotChocolate.Resolvers;
using HotChocolate.Types.Pagination;
using Inventory.Common.Application.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Common.Application.Graphql.Extensions
{
    public static class CursorPagingArgumentsExtensions
    {
#nullable enable
        public static CursorPagingArguments GetCursorPaggingArguments(
            this IResolverContext context,
            bool AllowBackwardPagination = true,
            int DefaultPageSize = 10)
        {
            var MaxPageSize = int.MaxValue;

            if (MaxPageSize < DefaultPageSize)
            {
                DefaultPageSize = MaxPageSize;
            }

            var first = context.ArgumentValue<int?>(CursorPagingArgumentNames.First);

            var last = AllowBackwardPagination
                ? context.ArgumentValue<int?>(CursorPagingArgumentNames.Last)
                : null;

            if (first is null && last is null)
            {
                first = DefaultPageSize;
            }

            return new CursorPagingArguments(
                first,
                last,
                context.ArgumentValue<string?>(CursorPagingArgumentNames.After),
                AllowBackwardPagination
                    ? context.ArgumentValue<string?>(CursorPagingArgumentNames.Before)
                    : null);

        }

        public static CursorPaginationRequest GetCursorPaggingRequest(
            this IResolverContext context)
        {
            var pagingArguments = context.GetCursorPaggingArguments();

            var size = default(int);
            if (pagingArguments.First.HasValue)
            {
                size = pagingArguments.First.Value;
            }
            else if (pagingArguments.Last.HasValue)
            {
                size = pagingArguments.Last.Value;
            }

            return new CursorPaginationRequest
            {
                After= pagingArguments.After,
                Before= pagingArguments.Before,
                FirstPage = pagingArguments.First.HasValue && string.IsNullOrEmpty(pagingArguments.After),
                LasttPage = !pagingArguments.First.HasValue && pagingArguments.Last.HasValue && string.IsNullOrEmpty(pagingArguments.Before),
                Size = size
            };

        }


        public static OffsetPagingArguments GetOffsetPagingArguments(
            this IResolverContext context,
            int DefaultPageSize = 50
        )
        {

            var MaxPageSize = int.MaxValue;

            if (MaxPageSize < DefaultPageSize)
            {
                DefaultPageSize = MaxPageSize;
            }

            var skip = context.ArgumentValue<int?>(OffsetPagingArgumentNames.Skip);
            var take = context.ArgumentValue<int?>(OffsetPagingArgumentNames.Take);

            if (!skip.HasValue) { skip = 0; }
            if (!take.HasValue) {  take = DefaultPageSize; }

            return new OffsetPagingArguments(skip, take);
        }

        public static OffsetPaginationRequest GetOffsetPagingRequest(
            this IResolverContext context
            )
        {

            var pagingArguments = context.GetOffsetPagingArguments();

            // calcul the page number from skip and take
            var page = pagingArguments.Skip.HasValue && pagingArguments.Take.HasValue
                ? (int)Math.Ceiling((double)pagingArguments.Skip.Value / pagingArguments.Take.Value) + 1
                : 1;

            return new OffsetPaginationRequest
            {
                Size = pagingArguments.Take.Value,
                Skip = pagingArguments.Skip.Value
            };
        }


#nullable disable




        internal static class CursorPagingArgumentNames
        {
            public const string First = "first";
            public const string After = "after";
            public const string Last = "last";
            public const string Before = "before";
        }

        internal static class OffsetPagingArgumentNames
        {
            public const string Skip = "skip";
            public const string Take = "take";
            public const string SortBy = "sortBy";
        }

    }
}
