using Disqord.Models;

namespace Disqord.Gateway
{
    public class CachedVoiceChannel : CachedNestableChannel, IVoiceChannel
    {
        public int MemberLimit { get; private set; }

        public int Bitrate { get; private set; }

        public CachedVoiceChannel(IGatewayClient client, Snowflake guildId, ChannelJsonModel model)
            : base(client, guildId, model)
        { }
    }
}
