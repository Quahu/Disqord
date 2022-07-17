using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Webhook;

/// <summary>
///     Defines the extension methods for <see cref="IWebhookClient"/>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class WebhookClientExtensions
{
    /// <summary>
    ///     Fetches this webhook.
    /// </summary>
    /// <param name="client"> The webhook client. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     with the result being the returned <see cref="IWebhook"/>.
    /// </returns>
    public static Task<IWebhook?> FetchAsync(this IWebhookClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.FetchWebhookAsync(client.Id, client.Token, options, cancellationToken);
    }

    /// <summary>
    ///     Modifies this webhook using the provided action.
    /// </summary>
    /// <param name="client"> The webhook client. </param>
    /// <param name="action"> The action specifying what properties to modify. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     with the result being the returned <see cref="IWebhook"/>.
    /// </returns>
    public static Task<IWebhook> ModifyAsync(this IWebhookClient client,
        Action<ModifyWebhookActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.ModifyWebhookAsync(client.Id, action, client.Token, options, cancellationToken);
    }

    /// <summary>
    ///     Deletes this webhook.
    /// </summary>
    /// <param name="client"> The webhook client. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public static Task DeleteAsync(this IWebhookClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.DeleteWebhookAsync(client.Id, client.Token, options, cancellationToken);
    }

    /// <summary>
    ///     Executes this webhook, i.e. sends a message from it.
    /// </summary>
    /// <param name="client"> The webhook client. </param>
    /// <param name="message"> The message to send. </param>
    /// <param name="threadId"> The ID of the thread (within the webhook's channel) in which this request should be performed. </param>
    /// <param name="wait"> Whether the API should return the sent message object. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     with the result being the sent <see cref="IUserMessage"/>
    ///     or <see langword="null"/> if <paramref name="wait"/> was not set to <see langword="true"/>.
    /// </returns>
    public static Task<IUserMessage?> ExecuteAsync(this IWebhookClient client,
        LocalWebhookMessage message,
        Snowflake? threadId = null, bool wait = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.ExecuteWebhookAsync(client.Id, client.Token, message, threadId, wait, options, cancellationToken);
    }

    /// <summary>
    ///     Fetch a message sent by this webhook.
    /// </summary>
    /// <param name="client"> The webhook client. </param>
    /// <param name="messageId"> The ID of the message to fetch. </param>
    /// <param name="threadId"> The ID of the thread (within the webhook's channel) in which this request should be performed. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public static Task<IUserMessage?> FetchMessageAsync(this IWebhookClient client,
        Snowflake messageId,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.FetchWebhookMessageAsync(client.Id, client.Token, messageId, threadId, options, cancellationToken);
    }

    /// <summary>
    ///     Modifies a message sent by this webhook.
    /// </summary>
    /// <param name="client"> The webhook client. </param>
    /// <param name="messageId"> The ID of the message to modify. </param>
    /// <param name="action"> The action specifying what properties to modify. </param>
    /// <param name="threadId"> The ID of the thread (within the webhook's channel) in which this request should be performed. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the request
    ///     with the result being the updated <see cref="IUserMessage"/>.
    /// </returns>
    public static Task<IUserMessage> ModifyMessageAsync(this IWebhookClient client,
        Snowflake messageId, Action<ModifyWebhookMessageActionProperties> action,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.ModifyWebhookMessageAsync(client.Id, client.Token, messageId, action, threadId, options, cancellationToken);
    }

    /// <summary>
    ///     Deletes a message sent by this webhook.
    /// </summary>
    /// <param name="client"> The webhook client. </param>
    /// <param name="messageId"> The ID of the message to modify. </param>
    /// <param name="threadId"> The ID of the thread (within the webhook's channel) in which this request should be performed. </param>
    /// <param name="options"> The optional request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the request.
    /// </returns>
    public static Task DeleteMessageAsync(this IWebhookClient client,
        Snowflake messageId,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return client.RestClient.DeleteWebhookMessageAsync(client.Id, client.Token, messageId, threadId, options, cancellationToken);
    }
}
