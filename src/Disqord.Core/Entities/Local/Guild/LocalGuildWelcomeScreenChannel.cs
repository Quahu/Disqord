using System;

namespace Disqord
{
    public class LocalGuildWelcomeScreenChannel : ILocalConstruct
    {
        public const int MaxDescriptionLength = 32;

        public Snowflake ChannelId { get; init; }

        public string Description { get; init; }

        public LocalEmoji Emoji { get; init; }

        public LocalGuildWelcomeScreenChannel()
        { }

        public LocalGuildWelcomeScreenChannel(Snowflake channelId, string description, LocalEmoji emoji = null)
        {
            ChannelId = channelId;
            Description = description;
            Emoji = emoji;
        }

        public virtual LocalGuildWelcomeScreenChannel Clone()
            => MemberwiseClone() as LocalGuildWelcomeScreenChannel;

        object ICloneable.Clone()
            => Clone();

        public void Validate()
        {
            if (Description.Length > MaxDescriptionLength)
                throw new InvalidOperationException($"The length of the description must not exceed {MaxDescriptionLength} characters.");
        }
    }
}
