using System;
using Disqord.Models;
using Qommon;

namespace Disqord
{
    public static class LocalAutoModerationActionMetadataExtensions
    {
        public static LocalAutoModerationActionMetadata WithChannelId(this LocalAutoModerationActionMetadata metadata, Snowflake channelId)
        {
            metadata.ChannelId = channelId;
            return metadata;
        }

        public static LocalAutoModerationActionMetadata WithTimeoutDuration(this LocalAutoModerationActionMetadata metadata, TimeSpan duration)
        {
            metadata.TimeoutDuration = duration;
            return metadata;
        }

        public static AutoModerationActionMetadataJsonModel ToModel(this LocalAutoModerationActionMetadata metadata)
        {
            return new AutoModerationActionMetadataJsonModel
            {
                ChannelId = metadata.ChannelId,
                DurationSeconds = Optional.Convert(metadata.TimeoutDuration, x => (int) x.TotalSeconds)
            };
        }
    }
}
