using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageReferenceJsonModel : JsonModel
{
    [JsonProperty("message_id")]
    public Optional<Snowflake> MessageId;

    [JsonProperty("channel_id")]
    public Optional<Snowflake> ChannelId;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("fail_if_not_exists")]
    public Optional<bool> FailIfNotExists;

    /// <inheritdoc />
    protected override void OnValidate()
    {
        OptionalGuard.HasValue(MessageId);
    }
}
