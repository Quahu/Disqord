using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AutoModerationActionJsonModel : JsonModel
{
    [JsonProperty("type")]
    public AutoModerationActionType Type;

    [JsonProperty("metadata")]
    public Optional<AutoModerationActionMetadataJsonModel> Metadata;
}