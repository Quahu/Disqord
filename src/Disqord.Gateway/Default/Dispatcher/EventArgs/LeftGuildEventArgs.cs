using System;

namespace Disqord.Gateway
{
    public class LeftGuildEventArgs : EventArgs
    {
        public IGatewayGuild Guild { get; }

        public LeftGuildEventArgs(IGatewayGuild guild)
        {
            if (guild == null)
                throw new ArgumentNullException(nameof(guild));

            Guild = guild;
        }
    }
}