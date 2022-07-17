using System;
using Qommon;

namespace Disqord;

public class LocalAutoModerationActionMetadata : ILocalConstruct<LocalAutoModerationActionMetadata>
{
    public Optional<Snowflake> ChannelId { get; set; }

    public Optional<TimeSpan> TimeoutDuration { get; set; }

    public LocalAutoModerationActionMetadata()
    { }

    protected LocalAutoModerationActionMetadata(LocalAutoModerationActionMetadata other)
    {
        ChannelId = other.ChannelId;
        TimeoutDuration = other.TimeoutDuration;
    }

    public virtual LocalAutoModerationActionMetadata Clone()
    {
        return new(this);
    }
}
