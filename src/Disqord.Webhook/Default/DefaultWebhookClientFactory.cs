using Disqord.Rest;
using Qommon;

namespace Disqord.Webhook.Default;

/// <inheritdoc/>
public class DefaultWebhookClientFactory : IWebhookClientFactory
{
    private readonly IRestClient _restClient;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultWebhookClientFactory"/> for the given REST client.
    /// </summary>
    /// <param name="restClient"> The REST client to use. </param>
    public DefaultWebhookClientFactory(IRestClient restClient)
    {
        _restClient = restClient;
    }

    /// <inheritdoc/>
    public IWebhookClient CreateClient(Snowflake id, string token)
    {
        Guard.IsNotNull(token);

        return new DefaultWebhookClient(_restClient, id, token);
    }
}