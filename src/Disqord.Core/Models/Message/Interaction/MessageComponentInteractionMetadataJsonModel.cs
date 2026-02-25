using Disqord.Serialization.Json;

namespace Disqord.Models;

public class MessageComponentInteractionMetadataJsonModel : MessageInteractionMetadataJsonModel
{
    [JsonProperty("interacted_message_id")]
    public Snowflake InteractedMessageId;
}
