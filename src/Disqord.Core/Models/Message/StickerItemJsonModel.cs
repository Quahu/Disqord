using Disqord.Serialization.Json;

namespace Disqord.Models;

public class StickerItemJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("format_type")]
    public StickerFormatType FormatType;
}
