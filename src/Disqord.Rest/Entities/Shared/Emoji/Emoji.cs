namespace Disqord
{
    public sealed class Emoji : IEmoji
    {
        public string Name { get; internal set; }

        public string ReactionFormat => Discord.ToMessageFormat(this);

        public string MessageFormat => Discord.ToReactionFormat(this);

        internal Emoji()
        { }

        public bool Equals(IEmoji other)
            => Discord.Comparers.Emoji.Equals(this, other);

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public override int GetHashCode()
            => Discord.Comparers.Emoji.GetHashCode(this);

        public override string ToString()
            => Name;
    }
}
