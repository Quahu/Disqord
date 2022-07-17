using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

public static class LocalAutoModerationActionMetadataExtensions
{
    public static TActionMetadata WithChannelId<TActionMetadata>(this TActionMetadata metadata, Snowflake channelId)
        where TActionMetadata : LocalAutoModerationActionMetadata
    {
        metadata.ChannelId = channelId;
        return metadata;
    }

    public static TActionMetadata WithTimeoutDuration<TActionMetadata>(this TActionMetadata metadata, TimeSpan duration)
        where TActionMetadata : LocalAutoModerationActionMetadata
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
