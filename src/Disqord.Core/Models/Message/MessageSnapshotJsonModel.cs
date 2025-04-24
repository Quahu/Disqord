using Disqord.Serialization.Json;

namespace Disqord.Models;

public class MessageSnapshotJsonModel : JsonModel
{
    [JsonProperty("message")]
    public MessageJsonModel Message = null!;
}