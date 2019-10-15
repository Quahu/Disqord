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
        {
            if (other == null)
                return false;

            if (other is ICustomEmoji)
                return false;

            return Name.Equals(other.Name);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is IEmoji emoji))
                return false;

            return Equals(emoji);
        }

        public override int GetHashCode()
            => Name.GetHashCode();
    }
}
