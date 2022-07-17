using Disqord.Rest;

namespace Disqord.OAuth2.Default;

/// <inheritdoc/>
public class DefaultBearerClient : IBearerClient
{
    IRestClient IBearerClient.RestClient => _restClient;

    private readonly IRestClient _restClient;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultBearerClient"/>.
    /// </summary>
    /// <param name="restClient"> The REST client to use. </param>
    public DefaultBearerClient(
        IRestClient restClient)
    {
        _restClient = restClient;
    }
}