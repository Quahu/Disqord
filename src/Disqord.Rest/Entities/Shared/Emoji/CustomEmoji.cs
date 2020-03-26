namespace Disqord
{
    public sealed class CustomEmoji : ICustomEmoji
    {
        public Snowflake Id { get; internal set; }

        public string Name { get; internal set; }

        public bool IsAnimated { get; internal set; }

        public string ReactionFormat => Discord.ToReactionFormat(this);

        public string MessageFormat => Discord.ToMessageFormat(this);

        public string Tag => MessageFormat;

        internal CustomEmoji()
        { }

        public string GetUrl(int size = 2048)
            => Discord.Cdn.GetCustomEmojiUrl(Id, IsAnimated, size);

        public bool Equals(IEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Discord.Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => MessageFormat;
    }
}
