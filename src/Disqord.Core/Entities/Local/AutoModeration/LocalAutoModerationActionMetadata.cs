using System;
using Qommon;

namespace Disqord
{
    public class LocalAutoModerationActionMetadata : ILocalConstruct
    {
        public Optional<Snowflake> ChannelId { get; set; }

        public Optional<TimeSpan> TimeoutDuration { get; set; }

        public LocalAutoModerationActionMetadata()
        { }

        public virtual LocalAutoModerationActionMetadata Clone()
            => MemberwiseClone() as LocalAutoModerationActionMetadata;

        object ICloneable.Clone()
            => Clone();
    }
}
