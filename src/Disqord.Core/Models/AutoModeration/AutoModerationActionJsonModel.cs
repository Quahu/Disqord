using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class AutoModerationActionJsonModel : JsonModel
{
    [JsonProperty("type")]
    public AutoModerationActionType Type;

    [JsonProperty("metadata")]
    public Optional<AutoModerationActionMetadataJsonModel> Metadata;

    protected override void OnValidate()
    {
        if (Type != AutoModerationActionType.BlockMessage)
            OptionalGuard.HasValue(Metadata);

        OptionalGuard.CheckValue(Metadata, metadata =>
        {
            switch (Type)
            {
                case AutoModerationActionType.BlockMessage:
                {
                    OptionalGuard.CheckValue(metadata.CustomMessage, message =>
                    {
                        Guard.HasSizeLessThanOrEqualTo(message, Discord.Limits.AutoModerationRule.ActionMetadata.MaxCustomMessageLength);
                    });
                    break;
                }
                case AutoModerationActionType.SendAlertMessage:
                {
                    OptionalGuard.HasValue(metadata.ChannelId);
                    break;
                }
                case AutoModerationActionType.Timeout:
                {
                    OptionalGuard.HasValue(metadata.DurationSeconds);
                    OptionalGuard.CheckValue(metadata.DurationSeconds, seconds =>
                    {
                        Guard.IsBetweenOrEqualTo(seconds, 0, Discord.Limits.AutoModerationRule.ActionMetadata.MaxDurationSeconds);
                    });
                    break;
                }
            }
        });
    }
}
