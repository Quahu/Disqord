using System;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Rest;

namespace Disqord.Interactivity.Pagination
{
    public abstract class PaginatorBase : IAsyncDisposable
    {
        public ICachedMessageChannel Channel { get; internal set; }

        public RestUserMessage Message { get; internal set; }

        public abstract Page DefaultPage { get; }

        protected internal abstract ValueTask InitialiseAsync();

        protected internal abstract ValueTask ExpiredAsync();

        protected internal abstract ValueTask<Page> GetPageAsync(ReactionAddedEventArgs e);

        public virtual ValueTask DisposeAsync()
            => default;
    }
}
