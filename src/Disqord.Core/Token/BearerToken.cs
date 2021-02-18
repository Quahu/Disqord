namespace Disqord
{
    /// <summary>
    ///     Represents a bearer token.
    /// </summary>
    public sealed class BearerToken : Token
    {
        /// <summary>
        ///     Instantiates a new <see cref="BearerToken"/>.
        /// </summary>
        /// <param name="value"> The bearer token. </param>
        public BearerToken(string value)
            : base(value)
        { }

        /// <inheritdoc/>
        public override string GetAuthorization()
            => $"Bearer {RawValue}";
    }
}
