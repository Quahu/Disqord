using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord authorization token.
    /// </summary>
    public abstract class Token
    {
        /// <summary>
        ///     A token that has no authorization. Can be used for webhooks, for example.
        /// </summary>
        public static readonly Token None = new NoToken();

        /// <summary>
        ///     Gets the raw token string.
        /// </summary>
        public string? RawValue { get; }

        private protected Token(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "The token must not be null or whitespace.");

            RawValue = value;
        }

        // No authorization ctor.
        private Token()
        { }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            if (obj is Token token)
                return RawValue == token.RawValue;

            if (obj is string rawValue)
                return RawValue == rawValue;

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
            => RawValue?.GetHashCode() ?? 0;

        public static bool operator ==(Token left, Token right)
            => left?.RawValue == right?.RawValue;

        public static bool operator !=(Token left, Token right)
            => left?.RawValue != right?.RawValue;

        /// <summary>
        ///     Gets the appropriate prefixed format of the token.
        /// </summary>
        public abstract string GetAuthorization();

        private class NoToken : Token
        {
            public NoToken()
            { }

            public override string GetAuthorization()
                => null;
        }
    }
}
