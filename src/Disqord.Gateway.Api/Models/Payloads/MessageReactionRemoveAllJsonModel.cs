using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class MessageReactionRemoveAllJsonModel : JsonModel
{
    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("message_id")]
    public Snowflake MessageId;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;
}