using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class MessageComponentInteractionDataJsonModel : InteractionDataJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("component_type")]
    public ComponentType ComponentType;

    [JsonProperty("values")]
    public Optional<string[]> Values;

    [JsonProperty("resolved")]
    public Optional<ApplicationCommandInteractionDataResolvedJsonModel> Resolved;
}
