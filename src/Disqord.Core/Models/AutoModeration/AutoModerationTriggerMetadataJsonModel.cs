using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AutoModerationTriggerMetadataJsonModel : JsonModel
{
    [JsonProperty("keyword_filter")]
    public Optional<string[]> KeywordFilter;

    [JsonProperty("presets")]
    public Optional<AutoModerationKeywordPresetType[]> Presets;
}
