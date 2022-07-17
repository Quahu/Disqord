using Disqord.Rest;

namespace Disqord.Webhook.Default;

/// <inheritdoc/>
public class DefaultWebhookClient : IWebhookClient
{
    /// <inheritdoc/>
    public Snowflake Id { get; }

    /// <inheritdoc/>
    public string Token { get; }

    private readonly IRestClient _restClient;

    IRestClient IWebhookClient.RestClient => _restClient;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultWebhookClient"/> for the given REST client
    ///     and ID and token of the webhook.
    /// </summary>
    /// <param name="restClient"> The REST client to use. </param>
    /// <param name="id"> The ID of the webhook. </param>
    /// <param name="token"> The token of the webhook. </param>
    public DefaultWebhookClient(
        IRestClient restClient,
        Snowflake id,
        string token)
    {
        _restClient = restClient;
        Id = id;
        Token = token;
    }
}
