using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

[JsonSkippedProperties("data")]
public class InteractionJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("application_id")]
    public Snowflake ApplicationId;

    [JsonProperty("type")]
    public InteractionType Type;

    [JsonIgnore]
    public Optional<InteractionDataJsonModel> Data; // Polymorphic deserialization in the InteractionJsonModel converter

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("channel")]
    public Optional<ChannelJsonModel> Channel;

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

    [JsonProperty("entitlements")]
    public EntitlementJsonModel[] Entitlements = null!;

    [JsonProperty("authorizing_integration_owners")]
    public Optional<Dictionary<ApplicationIntegrationType, Snowflake>> AuthorizingIntegrationOwners;

    [JsonProperty("context")]
    public Optional<InteractionContextType> Context;

    [JsonProperty("attachment_size_limit")]
    public Optional<int> AttachmentSizeLimit;
}
