using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class WebhookJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public WebhookType Type;

    [JsonProperty("guild_id")]
    public Optional<Snowflake?> GuildId;

    [JsonProperty("channel_id")]
    public Snowflake? ChannelId;

    [JsonProperty("user")]
    public Optional<UserJsonModel> User;

    [JsonProperty("name")]
    public string? Name;

    [JsonProperty("avatar")]
    public string? Avatar;

    [JsonProperty("token")]
    public Optional<string> Token;

    [JsonProperty("application_id")]
    public Snowflake? ApplicationId;

    [JsonProperty("source_guild")]
    public Optional<IJsonObject> SourceGuild;

    [JsonProperty("source_channel")]
    public Optional<IJsonObject> SourceChannel;

    [JsonProperty("url")]
    public Optional<string> Url;
}
