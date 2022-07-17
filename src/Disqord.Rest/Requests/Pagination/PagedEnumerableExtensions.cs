using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest.Pagination;

public static class PagedEnumerableExtensions
{
    /// <summary>
    ///     Flattens this <see cref="IPagedEnumerable{T}"/> into an <see cref="IAsyncEnumerable{T}"/>.
    /// </summary>
    /// <returns>
    ///     A flattened <see cref="IAsyncEnumerable{T}"/> of <typeparamref name="TEntity"/>s.
    /// </returns>
    public static async IAsyncEnumerable<TEntity> Flatten<TEntity>(this IPagedEnumerable<TEntity> source,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
        where TEntity : class
    {
        Guard.IsNotNull(source);

        await foreach (var page in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            foreach (var entity in page)
                yield return entity;
        }
    }

    /// <summary>
    ///     Flattens this <see cref="IPagedEnumerable{T}"/> into a single list.
    /// </summary>
    /// <returns>
    ///     A flattened list of <typeparamref name="TEntity"/>s.
    /// </returns>
    public static async Task<IReadOnlyList<TEntity>> FlattenAsync<TEntity>(this IPagedEnumerable<TEntity> source,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        Guard.IsNotNull(source);

        var list = new List<TEntity>();
        await foreach (var page in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            list.AddRange(page);

        return list;
    }

    public static async Task<TEntity?> FirstOrDefaultAsync<TEntity>(this IPagedEnumerable<TEntity> source,
        Predicate<TEntity> predicate, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        Guard.IsNotNull(source);
        Guard.IsNotNull(predicate);

        await foreach (var page in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            for (var i = 0; i < page.Count; i++)
            {
                var entity = page[i];
                if (predicate(entity))
                    return entity;
            }
        }

        return null;
    }

    public static async Task<IReadOnlyList<TEntity>?> FirstOrDefaultPageAsync<TEntity>(this IPagedEnumerable<TEntity> source,
        Predicate<IReadOnlyList<TEntity>> predicate, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        Guard.IsNotNull(source);
        Guard.IsNotNull(predicate);

        await foreach (var page in source.WithCancellation(cancellationToken).ConfigureAwait(false))
        {
            if (predicate(page))
                return page;
        }

        return null;
    }
}
