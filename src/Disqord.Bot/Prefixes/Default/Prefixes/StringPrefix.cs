using System;
using Qmmands;

namespace Disqord.Bot.Prefixes
{
    public sealed class StringPrefix : IPrefix
    {
        public string Value { get; }

        public StringComparison Comparison { get; }

        public StringPrefix(string value, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            Value = value;
            Comparison = comparison;
        }

        public bool TryFind(CachedUserMessage message, out string output)
            => CommandUtilities.HasPrefix(message.Content, Value, Comparison, out output);

        public override int GetHashCode()
            => Value.GetHashCode(Comparison);

        public override bool Equals(object obj)
        {
            if (obj is StringPrefix prefix)
                return Value.Equals(prefix.Value, Comparison);

            if (obj is string value)
                return Value.Equals(value, Comparison);

            return false;
        }

        public override string ToString()
            => Value;
    }
}
