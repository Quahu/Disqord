using Qmmands;

namespace Disqord.Bot.Prefixes
{
    public sealed class CharPrefix : IPrefix
    {
        public char Value { get; }

        public bool IsCaseSensitive { get; }

        public CharPrefix(char value, bool caseSensitive)
        {
            Value = value;
            IsCaseSensitive = caseSensitive;
        }

        internal bool InternalEquals(char character)
            => !IsCaseSensitive && char.ToUpperInvariant(character) == char.ToUpperInvariant(Value)
                || character == Value;

        public bool TryFind(CachedUserMessage message, out string output)
            => CommandUtilities.HasPrefix(message.Content, Value, IsCaseSensitive, out output);

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
