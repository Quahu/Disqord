using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class InteractionJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("application_id")]
    public Snowflake ApplicationId;

    [JsonProperty("type")]
    public InteractionType Type;

    [JsonProperty("data")]
    public Optional<InteractionDataJsonModel> Data;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("channel_id")]
    public Optional<Snowflake> ChannelId;

    [JsonProperty("member")]
    public Optional<MemberJsonModel> Member;

    [JsonProperty("user")]
    public Optional<UserJsonModel> User;

    [JsonProperty("token")]
    public string Token = null!;

    [JsonProperty("version")]
    public int Version;

    [JsonProperty("message")]
    public Optional<MessageJsonModel> Message;

    [JsonProperty("app_permissions")]
    public Optional<Permissions> AppPermissions;

    [JsonProperty("locale")]
    public Optional<string> Locale;

    [JsonProperty("guild_locale")]
    public Optional<string> GuildLocale;
}
