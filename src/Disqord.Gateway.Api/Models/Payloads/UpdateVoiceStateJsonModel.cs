using Disqord.Serialization.Json;

namespace Disqord.Gateway.Api.Models;

public class UpdateVoiceStateJsonModel : JsonModel
{
    [JsonProperty("guild_id")]
    public Snowflake GuildId;

    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("self_mute")]
    public bool SelfMute;

    [JsonProperty("self_deaf")]
    public bool SelfDeaf;
}