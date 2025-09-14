using System.Collections.Generic;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageInteractionMetadataJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public InteractionType Type;

    [JsonProperty("user")]
    public UserJsonModel User = null!;

    [JsonProperty("authorizing_integration_owners")]
    public Dictionary<ApplicationIntegrationType, Snowflake> AuthorizingIntegrationOwners = null!;

    [JsonProperty("original_response_message_id")]
    public Optional<Snowflake> OriginalResponseMessageId;
}
