using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models;

public class TypingStartJsonModel : JsonModel
{
    [JsonProperty("channel_id")]
    public Snowflake ChannelId;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("user_id")]
    public Snowflake UserId;

    /// <summary>
    ///     Unix time in seconds of when the user started typing.
    /// </summary>
    [JsonProperty("timestamp")]
    public int Timestamp;

    [JsonProperty("member")]
    public Optional<MemberJsonModel> Member;
}