using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Rest.Pagination
{
    public static class PaginationExtensions
    {
        /// <summary>
        ///     Flattens this <see cref="IPagedEnumerable{T}"/> into a single read-only list.
        /// </summary>
        /// <returns>
        ///     A flattened list of <typeparamref name="TEntity"/>s.
        /// </returns>
        public static async Task<IReadOnlyList<TEntity>> FlattenAsync<TEntity>(this IPagedEnumerable<TEntity> source)
            where TEntity : class
        {
            var list = new List<TEntity>();
            await foreach (var page in source.ConfigureAwait(false))
                list.AddRange(page);

            return list.ReadOnly();
        }

        public static async Task<TEntity> FirstOrDefaultAsync<TEntity>(this IPagedEnumerable<TEntity> source, Predicate<TEntity> predicate)
            where TEntity : class
        {
            await foreach (var page in source.ConfigureAwait(false))
            {
                for (var i = 0; i < page.Count; i++)
                {
                    var item = page[i];
                    if (predicate(item))
                        return item;
                }
            }

            return null;
        }

        public static async Task<IReadOnlyList<TEntity>> FirstOrDefaultPageAsync<TEntity>(this IPagedEnumerable<TEntity> source, Predicate<IReadOnlyList<TEntity>> predicate)
            where TEntity : class
        {
            await foreach (var page in source.ConfigureAwait(false))
            {
                if (predicate(page))
                    return page;
            }

            return null;
        }
    }
}
