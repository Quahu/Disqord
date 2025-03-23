using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api.Models;

public class InteractionCallbackResourceJsonModel : JsonModel
{
    [JsonProperty("type")]
    public InteractionResponseType Type;

    [JsonProperty("message")]
    public MessageJsonModel? Message;
}
