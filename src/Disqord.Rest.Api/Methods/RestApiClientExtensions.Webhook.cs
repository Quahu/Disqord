using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<WebhookJsonModel> CreateWebhookAsync(this IRestApiClient client, Snowflake channelId, CreateWebhookJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Webhook.CreateWebhook, channelId);
            return client.ExecuteAsync<WebhookJsonModel>(route, content, options);
        }

        public static Task<WebhookJsonModel[]> FetchChannelWebhooksAsync(this IRestApiClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Webhook.GetChannelWebhooks, channelId);
            return client.ExecuteAsync<WebhookJsonModel[]>(route, null, options);
        }

        public static Task<WebhookJsonModel[]> FetchGuildWebhooksAsync(this IRestApiClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Webhook.GetGuildWebhooks, guildId);
            return client.ExecuteAsync<WebhookJsonModel[]>(route, null, options);
        }

        public static Task<WebhookJsonModel> FetchWebhookAsync(this IRestApiClient client, Snowflake webhookId, string token = null, IRestRequestOptions options = null)
        {
            var route = token == null
                ? Format(Route.Webhook.GetWebhook, webhookId)
                : Format(Route.Webhook.GetWebhookWithToken, webhookId, token);
            return client.ExecuteAsync<WebhookJsonModel>(route, null, options);
        }

        public static Task<WebhookJsonModel> ModifyWebhookAsync(this IRestApiClient client, Snowflake webhookId, ModifyWebhookJsonRestRequestContent content, string token = null, IRestRequestOptions options = null)
        {
            var route = token == null
                ? Format(Route.Webhook.ModifyWebhook, webhookId)
                : Format(Route.Webhook.ModifyWebhookWithToken, webhookId, token);
            return client.ExecuteAsync<WebhookJsonModel>(route, content, options);
        }

        public static Task DeleteWebhookAsync(this IRestApiClient client, Snowflake webhookId, string token = null, IRestRequestOptions options = null)
        {
            var route = token == null
                ? Format(Route.Webhook.DeleteWebhook, webhookId)
                : Format(Route.Webhook.DeleteWebhookWithToken, webhookId, token);
            return client.ExecuteAsync(route, null, options);
        }

        public static Task<MessageJsonModel> ExecuteWebhookAsync(this IRestApiClient client, Snowflake webhookId, string token, ExecuteWebhookJsonRestRequestContent content, bool wait = false, IRestRequestOptions options = null)
        {
            // TODO: query param utility
            var route = Format(Route.Webhook.ExecuteWebhook, new[] { KeyValuePair.Create("wait", (object) wait) }, webhookId, token);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ExecuteWebhookAsync(this IRestApiClient client, Snowflake webhookId, string token, MultipartJsonPayloadRestRequestContent<ExecuteWebhookJsonRestRequestContent> content, bool wait = false, IRestRequestOptions options = null)
        {
            // TODO: query param utility
            var route = Format(Route.Webhook.ExecuteWebhook, new[] { KeyValuePair.Create("wait", (object) wait) }, webhookId, token);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyWebhookMessageAsync(this IRestApiClient client, Snowflake webhookId, string token, Snowflake messageId, ModifyWebhookMessageJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Webhook.ModifyWebhookMessage, webhookId, token, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task<MessageJsonModel> ModifyWebhookMessageAsync(this IRestApiClient client, Snowflake webhookId, string token, Snowflake messageId, MultipartJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent> content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Webhook.ModifyWebhookMessage, webhookId, token, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, content, options);
        }

        public static Task DeleteWebhookMessageAsync(this IRestApiClient client, Snowflake webhookId, string token, Snowflake messageId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Webhook.DeleteWebhookMessage, webhookId, token, messageId);
            return client.ExecuteAsync<MessageJsonModel>(route, null, options);
        }
    }
}
