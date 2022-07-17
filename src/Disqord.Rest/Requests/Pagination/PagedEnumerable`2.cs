using System;
using System.Threading;
using Disqord.Rest.Pagination;
using Qommon;

namespace Disqord.Rest;

/// <inheritdoc cref="IPagedEnumerable{TEntity}"/>
public class PagedEnumerable<TState, TEntity> : IPagedEnumerable<TEntity>
{
    private readonly Func<TState, CancellationToken, IPagedEnumerator<TEntity>> _factory;
    private readonly TState _state;

    public PagedEnumerable(Func<TState, CancellationToken, IPagedEnumerator<TEntity>> factory, TState state)
    {
        Guard.IsNotNull(factory);

        _factory = factory;
        _state = state;
    }

    /// <inheritdoc/>
    public IPagedEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken)
        => _factory(_state, cancellationToken);
}