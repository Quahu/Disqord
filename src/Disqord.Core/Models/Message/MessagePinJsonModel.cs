using System;
using Disqord.Serialization.Json;

namespace Disqord.Models;

public class MessagePinJsonModel
{
    [JsonProperty("pinned_at")]
    public DateTimeOffset PinnedAt;

    [JsonProperty("message")]
    public MessageJsonModel Message = null!;
}
