using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AutoModerationTriggerMetadataJsonModel : JsonModel
{
    [JsonProperty("keyword_filter")]
    public Optional<string[]> KeywordFilter;

    [JsonProperty("regex_patterns")]
    public Optional<string[]> RegexPatterns;

    [JsonProperty("presets")]
    public Optional<AutoModerationKeywordPresetType[]> Presets;

    [JsonProperty("allow_list")]
    public Optional<string[]> AllowList;

    [JsonProperty("mention_total_limit")]
    public Optional<int> MentionTotalLimit;

    [JsonProperty("mention_raid_protection_enabled")]
    public Optional<bool> MentionRaidProtectionEnabled;
}
