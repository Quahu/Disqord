using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Disqord.Webhook;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class WebhookClientFactoryExtensions
{
    /// <summary>
    ///     Creates an <see cref="IWebhookClient"/> from the specified URL.
    ///     This method simply extracts the ID and token from the URL and passes it down to <see cref="IWebhookClientFactory.CreateClient(Snowflake, string)"/>.
    /// </summary>
    /// <param name="factory"> The webhook client factory. </param>
    /// <param name="url"> The URL of the webhook. </param>
    /// <returns>
    ///     An <see cref="IWebhookClient"/> for the given URL.
    /// </returns>
    public static IWebhookClient CreateClient(this IWebhookClientFactory factory, string url)
    {
        var match = Regex.Match(url, @"discord(?:app)?.com\/api\/webhooks\/(?:(?<id>\d+)\/(?<token>.+))");
        if (!match.Success || string.IsNullOrWhiteSpace(url) || !Snowflake.TryParse(match.Groups["id"].Value, out var id))
            throw new UriFormatException();

        return factory.CreateClient(id, match.Groups["token"].Value);
    }
}