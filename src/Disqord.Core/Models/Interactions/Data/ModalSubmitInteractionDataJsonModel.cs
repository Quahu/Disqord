using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ModalSubmitInteractionDataJsonModel : InteractionDataJsonModel
{
    [JsonProperty("custom_id")]
    public string CustomId = null!;

    [JsonProperty("components")]
    public ModalBaseComponentJsonModel[] Components = null!;
}
