using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class CreateAutoModerationRuleJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("event_type")]
    public AutoModerationEventType EventType;

    [JsonProperty("trigger_type")]
    public AutoModerationRuleTrigger Trigger;

    [JsonProperty("trigger_metadata")]
    public Optional<AutoModerationTriggerMetadataJsonModel> TriggerMetadata;

    [JsonProperty("actions")]
    public AutoModerationActionJsonModel[] Actions = null!;

    [JsonProperty("enabled")]
    public Optional<bool> Enabled;

    [JsonProperty("exempt_roles")]
    public Optional<Snowflake[]> ExemptRoles;

    [JsonProperty("exempt_channels")]
    public Optional<Snowflake[]> ExemptChannels;

    /// <inheritdoc />
    protected override void OnValidate()
    {
        OptionalGuard.CheckValue(ExemptRoles, roles =>
        {
            Guard.HasSizeLessThanOrEqualTo(roles, Discord.Limits.AutoModerationRule.MaxExemptRoleAmount);
        });

        OptionalGuard.CheckValue(ExemptChannels, channels =>
        {
            Guard.HasSizeLessThanOrEqualTo(channels, Discord.Limits.AutoModerationRule.MaxExemptChannelAmount);
        });

        if (Trigger != AutoModerationRuleTrigger.Spam)
            OptionalGuard.HasValue(TriggerMetadata);

        OptionalGuard.CheckValue(TriggerMetadata, metadata =>
        {
            switch (Trigger)
            {
                case AutoModerationRuleTrigger.Keyword:
                {
                    OptionalGuard.CheckValue(metadata.KeywordFilter, filters =>
                    {
                        Guard.HasSizeLessThanOrEqualTo(filters, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxKeywordFilterAmount);

                        for (var i = 0; i < filters.Length; i++)
                            Guard.HasSizeBetweenOrEqualTo(filters[i], 1, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxKeywordFilterLength);
                    });

                    OptionalGuard.CheckValue(metadata.RegexPatterns, patterns =>
                    {
                        Guard.HasSizeLessThanOrEqualTo(patterns, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxRegexPatternsAmount);

                        for (var i = 0; i < patterns.Length; i++)
                            Guard.HasSizeBetweenOrEqualTo(patterns[i], 1, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxRegexPatternLength);
                    });

                    OptionalGuard.CheckValue(metadata.AllowList, allowList =>
                    {
                        Guard.HasSizeLessThanOrEqualTo(allowList, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxKeywordAllowListAmount);

                        for (var i = 0; i < allowList.Length; i++)
                            Guard.HasSizeBetweenOrEqualTo(allowList[i], 1, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxKeywordAllowedSubStringLength);
                    });
                    break;
                }

                case AutoModerationRuleTrigger.KeywordPreset:
                {
                    OptionalGuard.CheckValue(metadata.AllowList, allowList =>
                    {
                        Guard.HasSizeLessThanOrEqualTo(allowList, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxKeywordPresetAllowListAmount);

                        for (var i = 0; i < allowList.Length; i++)
                            Guard.HasSizeBetweenOrEqualTo(allowList[i], 1, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxKeywordPresetAllowedSubStringLength);
                    });
                    break;
                }

                case AutoModerationRuleTrigger.MentionSpam:
                {
                    OptionalGuard.HasValue(metadata.MentionTotalLimit);
                    OptionalGuard.CheckValue(metadata.MentionTotalLimit, limit =>
                    {
                        Guard.IsBetweenOrEqualTo(limit, 1, Discord.Limits.AutoModerationRule.TriggerMetadata.MaxMentionLimit);
                    });
                    break;
                }
            }
        });

        for (int i = 0; i < Actions.Length; i++)
            Actions[i].Validate();
    }
}
