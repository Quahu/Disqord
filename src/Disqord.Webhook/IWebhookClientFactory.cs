namespace Disqord.Webhook;

/// <summary>
///     Represents a factory used for instantiating <see cref="IWebhookClient"/>s
///     for given ID and token of the webhook.
/// </summary>
public interface IWebhookClientFactory
{
    /// <summary>
    ///     Creates an <see cref="IWebhookClient"/> from the specified ID and token.
    /// </summary>
    /// <param name="id"> The ID of the webhook. </param>
    /// <param name="token"> The token of the webhook. </param>
    /// <returns>
    ///     An <see cref="IWebhookClient"/> for the given parameters.
    /// </returns>
    IWebhookClient CreateClient(Snowflake id, string token);
}