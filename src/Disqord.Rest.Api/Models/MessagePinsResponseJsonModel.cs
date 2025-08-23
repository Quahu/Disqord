using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api.Models;

public class MessagePinsResponseJsonModel
{
    [JsonProperty("items")]
    public MessagePinJsonModel[] Items = null!;

    [JsonProperty("has_more")]
    public bool HasMore;
}
