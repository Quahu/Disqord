namespace Disqord
{
    public sealed class LocalEmoji : IEmoji
    {
        public string Name { get; }

        public string ReactionFormat => Discord.ToReactionFormat(this);

        public string MessageFormat => Discord.ToMessageFormat(this);

        public LocalEmoji(string unicode)
        {
            Name = unicode;
        }

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