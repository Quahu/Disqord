using System;
using System.Threading.Tasks;

namespace Disqord
{
    public abstract class DiscordClientExtension : IAsyncDisposable
    {
        public DiscordClientBase Client { get; }

        protected DiscordClientExtension(DiscordClientBase client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
        }

        protected internal abstract void Setup();

        public abstract ValueTask DisposeAsync();
    }
}