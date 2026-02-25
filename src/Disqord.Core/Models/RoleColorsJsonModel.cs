using Disqord.Serialization.Json;

namespace Disqord.Models;

public class RoleColorsJsonModel : JsonModel
{
    [JsonProperty("primary_color")]
    public int PrimaryColor;
    
    [JsonProperty("secondary_color")]
    public int? SecondaryColor;
    
    [JsonProperty("tertiary_color")]
    public int? TertiaryColor;
}