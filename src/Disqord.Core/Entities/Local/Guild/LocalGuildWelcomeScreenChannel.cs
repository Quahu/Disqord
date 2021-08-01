using System;

namespace Disqord
{
    public class LocalGuildWelcomeScreenChannel : ILocalConstruct
    {
        public Snowflake ChannelId { get; }

        public string Description { get; }

        public LocalEmoji Emoji { get; }

        public LocalGuildWelcomeScreenChannel(Snowflake channelId, string description, LocalEmoji emoji)
        {
            ChannelId = channelId;
            Description = description;
            Emoji = emoji;
        }

        public LocalGuildWelcomeScreenChannel(Snowflake channelId, string description)
        {
            ChannelId = channelId;
            Description = description;
        }

        public virtual LocalGuildWelcomeScreenChannel Clone()
            => MemberwiseClone() as LocalGuildWelcomeScreenChannel;

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        { }
    }
}
