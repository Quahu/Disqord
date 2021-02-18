using System;

namespace Disqord
{
    /// <summary>
    ///     Represents a user account token.
    /// </summary>
    [Obsolete("The usage of user account tokens is not supported and will result in the account's termination.", true)]
    public sealed class UserToken : Token
    {
        /// <summary>
        ///     Instantiates a new <see cref="UserToken"/>.
        /// </summary>
        /// <param name="value"> The user token. </param>
        public UserToken(string value)
            : base(value)
        { }

        /// <inheritdoc/>
        public override string GetAuthorization()
            => RawValue;
    }
}
