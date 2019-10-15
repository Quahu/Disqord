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
        {
            if (other == null)
                return false;

            if (!(other is ICustomEmoji customEmoji))
                return false;

            return Id.Equals(customEmoji.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is ICustomEmoji emoji))
                return false;

            return Equals(emoji);
        }

        public override int GetHashCode()
            => Id.GetHashCode();
    }
}
