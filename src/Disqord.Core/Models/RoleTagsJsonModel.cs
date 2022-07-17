using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class RoleTagsJsonModel : JsonModel
{
    [JsonProperty("bot_id")]
    public Optional<Snowflake> BotId;

    [JsonProperty("integration_id")]
    public Optional<Snowflake> IntegrationId;

    [JsonProperty("premium_subscriber")]
    public Optional<bool?> PremiumSubscriber;
}