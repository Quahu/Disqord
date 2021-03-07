using System;

namespace Disqord.Gateway
{
    public class JoinedGuildEventArgs : EventArgs
    {
        public IGatewayGuild Guild { get; }

        public JoinedGuildEventArgs(IGatewayGuild guild)
        {
            if (guild == null)
                throw new ArgumentNullException(nameof(guild));

            Guild = guild;
        }
    }
}