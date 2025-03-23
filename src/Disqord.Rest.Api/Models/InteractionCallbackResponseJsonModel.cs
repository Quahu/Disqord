using Disqord.Serialization.Json;

namespace Disqord.Rest.Api.Models;

public class InteractionCallbackResponseJsonModel : JsonModel
{
    [JsonProperty("interaction")]
    public CallbackInteractionJsonModel Interaction = null!;

    [JsonProperty("resource")]
    public InteractionCallbackResourceJsonModel? Resource;
}
