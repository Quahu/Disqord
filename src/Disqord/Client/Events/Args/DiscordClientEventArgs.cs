using System;

namespace Disqord.Events
{
    public abstract class DiscordEventArgs : EventArgs
    {
        public DiscordClient Client { get; }

        internal DiscordEventArgs(DiscordClient client)
        {
            Client = client;
        }
    }
}
