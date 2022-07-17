using System;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task<IWebhook> ModifyAsync(this IWebhook webhook,
        Action<ModifyWebhookActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = webhook.GetRestClient();
        return client.ModifyWebhookAsync(webhook.Id, action, null, options, cancellationToken);
    }

    public static Task<IWebhook> ModifyWithTokenAsync(this IWebhook webhook,
        Action<ModifyWebhookActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = webhook.GetRestClient();
        return client.ModifyWebhookAsync(webhook.Id, action, webhook.Token, options, cancellationToken);
    }

    public static Task DeleteAsync(this IWebhook webhook,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = webhook.GetRestClient();
        return client.DeleteWebhookAsync(webhook.Id, null, options, cancellationToken);
    }

    public static Task DeleteWithTokenAsync(this IWebhook webhook,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = webhook.GetRestClient();
        return client.DeleteWebhookAsync(webhook.Id, webhook.Token, options, cancellationToken);
    }

    public static Task<IUserMessage?> ExecuteAsync(this IWebhook webhook,
        LocalWebhookMessage message,
        Snowflake? threadId = null, bool wait = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(webhook.Token);

        var client = webhook.GetRestClient();
        return client.ExecuteWebhookAsync(webhook.Id, webhook.Token, message, threadId, wait, options, cancellationToken);
    }

    public static Task<IUserMessage?> FetchMessageAsync(this IWebhook webhook,
        Snowflake messageId,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(webhook.Token);

        var client = webhook.GetRestClient();
        return client.FetchWebhookMessageAsync(webhook.Id, webhook.Token, messageId, threadId, options, cancellationToken);
    }

    public static Task<IUserMessage> ModifyMessageAsync(this IWebhook webhook,
        Snowflake messageId, Action<ModifyWebhookMessageActionProperties> action,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(webhook.Token);

        var client = webhook.GetRestClient();
        return client.ModifyWebhookMessageAsync(webhook.Id, webhook.Token, messageId, action, threadId, options, cancellationToken);
    }

    public static Task DeleteMessageAsync(this IWebhook webhook,
        Snowflake messageId,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(webhook.Token);

        var client = webhook.GetRestClient();
        return client.DeleteWebhookMessageAsync(webhook.Id, webhook.Token, messageId, threadId, options, cancellationToken);
    }
}
