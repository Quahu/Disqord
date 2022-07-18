using System;
using Disqord.Models;
using Qommon;

namespace Disqord;

public class LocalAutoModerationAction : ILocalConstruct<LocalAutoModerationAction>, IJsonConvertible<AutoModerationActionJsonModel>
{
    public static LocalAutoModerationAction BlockMessage()
    {
        return new LocalAutoModerationAction
        {
            Type = AutoModerationActionType.BlockMessage
        };
    }

    public static LocalAutoModerationAction SendAlertMessage(Snowflake channelId)
    {
        return new LocalAutoModerationAction
        {
            Type = AutoModerationActionType.SendAlertMessage,
            Metadata = new LocalAutoModerationActionMetadata
            {
                ChannelId = channelId
            }
        };
    }

    public static LocalAutoModerationAction Timeout(TimeSpan duration)
    {
        return new LocalAutoModerationAction
        {
            Type = AutoModerationActionType.Timeout,
            Metadata = new LocalAutoModerationActionMetadata
            {
                TimeoutDuration = duration
            }
        };
    }

    public Optional<AutoModerationActionType> Type { get; set; }

    public Optional<LocalAutoModerationActionMetadata> Metadata { get; set; }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAutoModerationAction"/>.
    /// </summary>
    public LocalAutoModerationAction()
    { }

    /// <summary>
    ///     Instantiates a new <see cref="LocalAutoModerationAction"/> with the properties copied from another instance.
    /// </summary>
    /// <param name="other"> The other instance to copy properties from. </param>
    protected LocalAutoModerationAction(LocalAutoModerationAction other)
    {
        Type = other.Type;
        Metadata = other.Metadata.Clone();
    }

    /// <inheritdoc/>
    public virtual LocalAutoModerationAction Clone()
    {
        return new(this);
    }

    /// <inheritdoc />
    public AutoModerationActionJsonModel ToModel()
    {
        OptionalGuard.HasValue(Type);

        return new AutoModerationActionJsonModel
        {
            Type = Type.Value,
            Metadata = Optional.Convert(Metadata, metadata => metadata.ToModel())
        };
    }
}
