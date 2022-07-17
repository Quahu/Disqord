using Disqord.Serialization.Json;

namespace Disqord.Models;

public class WelcomeScreenJsonModel : JsonModel
{
    [JsonProperty("description")]
    public string? Description;

    [JsonProperty("welcome_channels")]
    public WelcomeScreenChannelJsonModel[] Channels = null!;
}
