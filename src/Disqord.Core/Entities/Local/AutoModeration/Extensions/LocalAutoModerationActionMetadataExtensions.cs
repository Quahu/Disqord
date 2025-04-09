using System;
using System.ComponentModel;

namespace Disqord;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class LocalAutoModerationActionMetadataExtensions
{
    public static TActionMetadata WithCustomMessage<TActionMetadata>(this TActionMetadata metadata, string customMessage)
        where TActionMetadata : LocalAutoModerationActionMetadata
    {
        metadata.CustomMessage = customMessage;
        return metadata;
    }

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
}
