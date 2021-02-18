using System;

namespace Disqord.Gateway
{
    public class VoiceServerUpdatedEventArgs : EventArgs
    {
        public Snowflake GuildId { get; }

        public string Token { get; }

        public string Endpoint { get; }

        public VoiceServerUpdatedEventArgs(Snowflake guildId, string token, string endpoint)
        {
            GuildId = guildId;
            Token = token;
            Endpoint = endpoint;
        }
    }
}
