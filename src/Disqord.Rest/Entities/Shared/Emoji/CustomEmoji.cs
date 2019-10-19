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
            => Discord.GetCustomEmojiUrl(Id, IsAnimated, size);

        public bool Equals(IEmoji other)
            => other is ICustomEmoji emoji && Id == emoji.Id;

        public override bool Equals(object obj)
            => obj is ICustomEmoji emoji && Id == emoji.Id;

        public static bool operator ==(CustomEmoji left, CustomEmoji right)
            => left.Id == right.Id;

        public static bool operator !=(CustomEmoji left, CustomEmoji right)
            => left.Id != right.Id;

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}
