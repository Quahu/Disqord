using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageModalSubmitInteractionMetadataJsonModel : MessageInteractionMetadataJsonModel
{
    [JsonProperty("triggering_interaction_metadata")]
    public MessageInteractionMetadataJsonModel TriggeringInteractionMetadata = null!;
}
