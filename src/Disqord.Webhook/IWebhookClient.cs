using System.ComponentModel;
using Disqord.Rest;

namespace Disqord.Webhook;

/// <summary>
///     Represents a REST client wrapper for a specific webhook
///     which simplifies performing operations on said webhook.
/// </summary>
public interface IWebhookClient
{
    /// <summary>
    ///     Gets the underlying REST client.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    IRestClient RestClient { get; }

    /// <summary>
    ///     Gets the ID of the webhook this client wraps.
    /// </summary>
    Snowflake Id { get; }

    /// <summary>
    ///     Gets the token of the webhook this client wraps.
    /// </summary>
    string Token { get; }
}