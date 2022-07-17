using System;

namespace Disqord;

public abstract partial class Token
{
    /// <summary>
    ///     A token that has no authorization. Can be used for webhooks, for example.
    /// </summary>
    public static readonly Token None = new NoToken();

    /// <summary>
    ///     Creates a new <see cref="BotToken"/> from the provided <see cref="string"/>.
    /// </summary>
    /// <param name="token"> The raw token <see cref="string"/>. </param>
    /// <returns>
    ///     A <see cref="Token"/>instance representing a bot token.
    /// </returns>
    public static BotToken Bot(string token)
    {
        return new(token);
    }

    /// <summary>
    ///     Creates a new <see cref="BearerToken"/> from the provided <see cref="string"/>.
    /// </summary>
    /// <param name="token"> The raw token <see cref="string"/>. </param>
    /// <returns>
    ///     A <see cref="Token"/>instance representing a bearer token.
    /// </returns>
    public static BearerToken Bearer(string token)
    {
        return new(token);
    }

    /// <summary>
    ///     Creates a new <see cref="UserToken"/> from the provided <see cref="string"/>.
    /// </summary>
    /// <param name="token"> The raw token <see cref="string"/>. </param>
    /// <returns>
    ///     A <see cref="Token"/>instance representing a user token.
    /// </returns>
    [Obsolete("The usage of user account tokens is not supported and will result in the account's termination.", true)]
    public static UserToken User(string token)
    {
        return new(token);
    }

    private class NoToken : Token
    {
        public override string? GetAuthorization()
            => null;
    }
}