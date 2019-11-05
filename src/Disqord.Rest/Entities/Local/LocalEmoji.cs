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

        public override int GetHashCode()
            => Name.GetHashCode();

        public override bool Equals(object obj)
            => obj is IEmoji emoji && Equals(emoji);

        public bool Equals(IEmoji other)
        {
            if (other == null)
                return false;

            if (other is ICustomEmoji)
                return false;

            return Name.Equals(other.Name);
        }

        public override string ToString()
            => MessageFormat;
    }
}