namespace Disqord.OAuth2;

/// <summary>
///     Represents a factory used for creating instances of <see cref="IBearerClient"/>.
/// </summary>
public interface IBearerClientFactory
{
    /// <summary>
    ///     Creates an <see cref="IBearerClient"/> for the specified user's bearer token.
    /// </summary>
    /// <param name="token"> The bearer token of the user. </param>
    /// <returns>
    ///     An <see cref="IBearerClient"/>.
    /// </returns>
    IBearerClient CreateClient(BearerToken token);
}