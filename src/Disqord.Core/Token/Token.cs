using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a Discord authorization token.
    /// </summary>
    public abstract partial class Token
    {
        /// <summary>
        ///     Gets the raw token string.
        /// </summary>
        public string RawValue { get; }

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
        public override bool Equals(object obj)
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

        /// <summary>
        ///     Gets the appropriately prefixed format of the token used for authorization headers.
        /// </summary>
        public abstract string GetAuthorization();

        public static bool operator ==(Token left, Token right)
            => left?.RawValue == right?.RawValue;

        public static bool operator !=(Token left, Token right)
            => left?.RawValue != right?.RawValue;
    }
}
