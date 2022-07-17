using System;
using System.Threading;
using Disqord.Rest.Pagination;

namespace Disqord.Rest;

public static class PagedEnumerable
{
    public static PagedEnumerable<TState, TEntity> Create<TState, TEntity>(Func<TState, CancellationToken, IPagedEnumerator<TEntity>> factory, TState state)
        => new(factory, state);
}