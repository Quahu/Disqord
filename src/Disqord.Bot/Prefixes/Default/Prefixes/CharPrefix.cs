namespace Disqord.Bot.Prefixes
{
    public sealed class CharPrefix : IPrefix
    {
        public char Value { get; }

        public bool IgnoreCase { get; }

        public CharPrefix(char value, bool ignoreCase)
        {
            Value = value;
            IgnoreCase = ignoreCase;
        }

        internal bool InternalEquals(char character)
            => IgnoreCase && char.ToUpperInvariant(character) == char.ToUpperInvariant(Value)
                || character == Value;

        public bool TryFind(CachedUserMessage message, out string output)
        {
            if (InternalEquals(message.Content[0]))
            {
                output = message.Content.Substring(1);
                return true;
            }

            output = null;
            return false;
        }

        public override int GetHashCode()
            => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if (obj is CharPrefix prefix)
                return InternalEquals(prefix.Value);

            if (obj is char value)
                return InternalEquals(value);

            return false;
        }

        public override string ToString()
            => Value.ToString();
    }
}
