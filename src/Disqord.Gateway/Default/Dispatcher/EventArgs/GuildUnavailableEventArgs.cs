using System;

namespace Disqord.Gateway
{
    public class GuildUnavailableEventArgs : EventArgs
    {
        public IGatewayGuild Guild { get; }

        public GuildUnavailableEventArgs(IGatewayGuild guild)
        {
            if (guild == null)
                throw new ArgumentNullException(nameof(guild));

            Guild = guild;
        }
    }
}
