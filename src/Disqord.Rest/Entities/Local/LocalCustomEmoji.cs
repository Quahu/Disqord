namespace Disqord
{
    public sealed class LocalCustomEmoji : ICustomEmoji
    {
        public Snowflake Id { get; }

        public string Name { get; }

        public bool IsAnimated { get; }

        public string ReactionFormat => Discord.ToReactionFormat(this);

        public string MessageFormat => Discord.ToMessageFormat(this);

        public string Tag => MessageFormat;

        public LocalCustomEmoji(Snowflake id, string name, bool isAnimated)
        {
            Id = id;
            Name = name;
            IsAnimated = isAnimated;
        }

        public string GetUrl(int size = 2048)
            => Discord.GetCustomEmojiUrl(Id, IsAnimated, size);

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

        //public static bool TryParse(string value, out LocalCustomEmoji result)
        //    => Discord.TryParseEmoji(value, out result);

        //public static implicit operator LocalCustomEmoji(string value)
        //    => new LocalCustomEmoji(value);
    }
}