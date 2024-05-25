using Disqord.Serialization.Json;

namespace Disqord.Models;

public class DefaultValueJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public DefaultSelectionValueType Type;
}
