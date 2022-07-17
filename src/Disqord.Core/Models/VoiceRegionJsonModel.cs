using Disqord.Serialization.Json;

namespace Disqord.Models;

public class VoiceRegionJsonModel : JsonModel
{
    [JsonProperty("id")]
    public string Id = null!;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("vip")]
    public bool Vip;

    [JsonProperty("optimal")]
    public bool Optimal;

    [JsonProperty("deprecated")]
    public bool Deprecated;

    [JsonProperty("custom")]
    public bool Custom;
}
