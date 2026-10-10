using Disqord.Rest;
using Disqord.Rest.Api.Default;

namespace Disqord.OAuth2.Default;

/// <inheritdoc/>
public class DefaultBearerClient : IBearerClient
{
    IRestClient IBearerClient.RestClient => _restClient;

    private readonly IRestClient _restClient;
    private readonly DefaultRestRateLimiter? _rateLimiter;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultBearerClient"/>.
    /// </summary>
    /// <param name="restClient"> The REST client to use. </param>
    public DefaultBearerClient(
        IRestClient restClient)
        : this(restClient, null)
    { }

    /// <summary>
    ///     Instantiates a new <see cref="DefaultBearerClient"/> that disposes the specified rate-limiter when it is disposed.
    /// </summary>
    /// <param name="restClient"> The REST client to use. </param>
    /// <param name="rateLimiter"> The rate-limiter to dispose together with this client. </param>
    public DefaultBearerClient(
        IRestClient restClient,
        DefaultRestRateLimiter? rateLimiter)
    {
        _restClient = restClient;
        _rateLimiter = rateLimiter;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _rateLimiter?.Dispose();
    }
}