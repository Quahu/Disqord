using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<WebhookJsonModel> CreateWebhookAsync(this IRestApiClient client,
        Snowflake channelId, CreateWebhookJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Webhook.CreateWebhook, channelId);
        return client.ExecuteAsync<WebhookJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<WebhookJsonModel[]> FetchChannelWebhooksAsync(this IRestApiClient client,
        Snowflake channelId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Webhook.GetChannelWebhooks, channelId);
        return client.ExecuteAsync<WebhookJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<WebhookJsonModel[]> FetchGuildWebhooksAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Webhook.GetGuildWebhooks, guildId);
        return client.ExecuteAsync<WebhookJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<WebhookJsonModel> FetchWebhookAsync(this IRestApiClient client,
        Snowflake webhookId, string? token = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = token == null
            ? Format(Route.Webhook.GetWebhook, webhookId)
            : Format(Route.Webhook.GetWebhookWithToken, webhookId, token);

        return client.ExecuteAsync<WebhookJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<WebhookJsonModel> ModifyWebhookAsync(this IRestApiClient client,
        Snowflake webhookId, ModifyWebhookJsonRestRequestContent content, string? token = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = token == null
            ? Format(Route.Webhook.ModifyWebhook, webhookId)
            : Format(Route.Webhook.ModifyWebhookWithToken, webhookId, token);

        return client.ExecuteAsync<WebhookJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteWebhookAsync(this IRestApiClient client,
        Snowflake webhookId, string? token = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = token == null
            ? Format(Route.Webhook.DeleteWebhook, webhookId)
            : Format(Route.Webhook.DeleteWebhookWithToken, webhookId, token);

        return client.ExecuteAsync(route, null, options, cancellationToken);
    }

    public static Task<MessageJsonModel?> ExecuteWebhookAsync(this IRestApiClient client,
        Snowflake webhookId, string token, ExecuteWebhookJsonRestRequestContent content,
        Snowflake? threadId = null, bool wait = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        // TODO: query param utility
        var queryParameters = new KeyValuePair<string, object>[threadId != null ? 2 : 1];
        queryParameters[0] = KeyValuePair.Create("wait", (object) wait);

        if (threadId != null)
            queryParameters[1] = new("thread_id", threadId.Value);

        var route = Format(Route.Webhook.ExecuteWebhook, queryParameters, webhookId, token);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken)!;
    }

    public static Task<MessageJsonModel?> ExecuteWebhookAsync(this IRestApiClient client,
        Snowflake webhookId, string token, AttachmentJsonPayloadRestRequestContent<ExecuteWebhookJsonRestRequestContent> content,
        Snowflake? threadId = null, bool wait = false,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        // TODO: query param utility
        var queryParameters = new KeyValuePair<string, object>[threadId != null ? 2 : 1];
        queryParameters[0] = KeyValuePair.Create("wait", (object) wait);

        if (threadId != null)
            queryParameters[1] = new("thread_id", threadId.Value);

        var route = Format(Route.Webhook.ExecuteWebhook, queryParameters, webhookId, token);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken)!;
    }

    public static Task<MessageJsonModel> FetchWebhookMessageAsync(this IRestApiClient client,
        Snowflake webhookId, string token, Snowflake messageId,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = threadId != null
            ? new KeyValuePair<string, object>[] { new("thread_id", threadId.Value) }
            : null;

        var route = Format(Route.Webhook.GetWebhookMessage, queryParameters, webhookId, token, messageId);
        return client.ExecuteAsync<MessageJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<MessageJsonModel> ModifyWebhookMessageAsync(this IRestApiClient client,
        Snowflake webhookId, string token, Snowflake messageId, ModifyWebhookMessageJsonRestRequestContent content,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = threadId != null
            ? new KeyValuePair<string, object>[] { new("thread_id", threadId.Value) }
            : null;

        var route = Format(Route.Webhook.ModifyWebhookMessage, queryParameters, webhookId, token, messageId);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<MessageJsonModel> ModifyWebhookMessageAsync(this IRestApiClient client,
        Snowflake webhookId, string token, Snowflake messageId, AttachmentJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = threadId != null
            ? new KeyValuePair<string, object>[] { new("thread_id", threadId.Value) }
            : null;

        var route = Format(Route.Webhook.ModifyWebhookMessage, queryParameters, webhookId, token, messageId);
        return client.ExecuteAsync<MessageJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteWebhookMessageAsync(this IRestApiClient client,
        Snowflake webhookId, string token, Snowflake messageId,
        Snowflake? threadId = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var queryParameters = threadId != null
            ? new KeyValuePair<string, object>[] { new("thread_id", threadId.Value) }
            : null;

        var route = Format(Route.Webhook.DeleteWebhookMessage, queryParameters, webhookId, token, messageId);
        return client.ExecuteAsync<MessageJsonModel>(route, null, options, cancellationToken);
    }
}
