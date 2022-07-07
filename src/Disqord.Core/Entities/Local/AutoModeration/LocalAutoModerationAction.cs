using System;
using Qommon;

namespace Disqord
{
    public class LocalAutoModerationAction : ILocalConstruct
    {
        public static LocalAutoModerationAction BlockMessage()
        {
            return new LocalAutoModerationAction(AutoModerationActionType.BlockMessage);
        }

        public static LocalAutoModerationAction SendAlertMessage(Snowflake channelId)
        {
            return new LocalAutoModerationAction(AutoModerationActionType.SendAlertMessage,
                new LocalAutoModerationActionMetadata().WithChannelId(channelId));
        }

        public static LocalAutoModerationAction Timeout(TimeSpan duration)
        {
            return new LocalAutoModerationAction(AutoModerationActionType.Timeout,
                new LocalAutoModerationActionMetadata().WithTimeoutDuration(duration));
        }

        public Optional<AutoModerationActionType> Type { get; set; }

        public Optional<LocalAutoModerationActionMetadata> Metadata { get; set; }

        public LocalAutoModerationAction()
        { }

        public LocalAutoModerationAction(AutoModerationActionType type)
        {
            Type = type;
        }

        public LocalAutoModerationAction(AutoModerationActionType type, LocalAutoModerationActionMetadata metadata)
        {
            Type = type;
            Metadata = metadata;
        }

        public virtual LocalAutoModerationAction Clone()
            => MemberwiseClone() as LocalAutoModerationAction;

        object ICloneable.Clone()
            => Clone();
    }
}
