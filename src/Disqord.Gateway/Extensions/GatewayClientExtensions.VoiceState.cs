using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway;

public static partial class GatewayClientExtensions
{
    public static Task SetVoiceStateAsync(this IShard shard, Snowflake guildId, Snowflake? channelId, bool selfMute = false, bool selfDeafen = false, CancellationToken cancellationToken = default)
    {
        return shard.InternalSetVoiceStateAsync(guildId, channelId, selfMute, selfDeafen, cancellationToken);
    }

    private static Task InternalSetVoiceStateAsync(this IShard shard, Snowflake guildId, Snowflake? channelId, bool selfMute, bool selfDeafen, CancellationToken cancellationToken)
    {
        var payload = new GatewayPayloadJsonModel
        {
            Op = GatewayPayloadOperation.UpdateVoiceState,
            D = new UpdateVoiceStateJsonModel
            {
                GuildId = guildId,
                ChannelId = channelId,
                SelfMute = selfMute,
                SelfDeaf = selfDeafen
            }
        };

        return shard.SendAsync(payload, cancellationToken);
    }
}
