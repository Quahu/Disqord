using System;

namespace Disqord.Gateway
{
    public class GuildAvailableEventArgs : EventArgs
    {
        public IGatewayGuild Guild { get; }

        public GuildAvailableEventArgs(IGatewayGuild guild)
        {
            if (guild == null)
                throw new ArgumentNullException(nameof(guild));

            Guild = guild;
        }
    }
}
