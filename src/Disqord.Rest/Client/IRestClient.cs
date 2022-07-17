using System.Collections.Generic;
using Disqord.Api;
using Disqord.Rest.Api;

namespace Disqord.Rest;

/// <summary>
///     Represents a high-level client for the Discord REST API.
/// </summary>
public interface IRestClient : IClient
{
    /// <summary>
    ///     Gets the cached direct channels.
    /// </summary>
    /// <remarks>
    ///     This exists to prevent multiple calls to <see cref="RestClientExtensions.CreateDirectChannelAsync"/>
    ///     for the same user as direct channels never actually get deleted.
    /// </remarks>
    IDictionary<Snowflake, IDirectChannel>? DirectChannels { get; }

    /// <inheritdoc cref="IClient.ApiClient"/>
    new IRestApiClient ApiClient { get; }

    IApiClient IClient.ApiClient => ApiClient;
}
