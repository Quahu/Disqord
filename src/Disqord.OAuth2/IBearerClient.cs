using System.ComponentModel;
using Disqord.Rest;

namespace Disqord.OAuth2;

/// <summary>
///     Represents a REST client wrapper for a specific bearer token.
/// </summary>
public interface IBearerClient
{
    /// <summary>
    ///     Gets the underlying REST client.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    IRestClient RestClient { get; }
}