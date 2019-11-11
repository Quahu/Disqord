using System;
using System.Threading.Tasks;

namespace Disqord
{
    public abstract class DiscordClientExtension : IAsyncDisposable
    {
        public DiscordClientBase Client { get; internal set; }

        protected internal abstract ValueTask SetupAsync();

        public virtual ValueTask DisposeAsync()
            => default;
    }
}