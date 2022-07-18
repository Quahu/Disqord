using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalAutoModerationActionMetadata : ILocalConstruct<LocalAutoModerationActionMetadata>, IJsonConvertible<AutoModerationActionMetadataJsonModel>
{
    public Optional<Snowflake> ChannelId { get; set; }

    public Optional<TimeSpan> TimeoutDuration { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAutoModerationActionMetadata"/>.
    /// </summary>
    public LocalAutoModerationActionMetadata()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAutoModerationActionMetadata"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalAutoModerationActionMetadata(LocalAutoModerationActionMetadata other)
    {
        ChannelId = other.ChannelId;
        TimeoutDuration = other.TimeoutDuration;
    }

    /// <inheritdoc/>
    public virtual LocalAutoModerationActionMetadata Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public AutoModerationActionMetadataJsonModel ToModel()
    {
        return new AutoModerationActionMetadataJsonModel
        {
            ChannelId = ChannelId,
            DurationSeconds = Optional.Convert(TimeoutDuration, x => (int) x.TotalSeconds)
        };
    }
}
