using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;
using Disqord.Rest.Api;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IWebhook> CreateWebhookAsync(this IRestClient client,
            Snowflake channelId, string name, Action<CreateWebhookActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new CreateWebhookActionProperties();
            action?.Invoke(properties);
            var content = new CreateWebhookJsonRestRequestContent(name)
            {
                Avatar = properties.Avatar
            };

            var model = await client.ApiClient.CreateWebhookAsync(channelId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientWebhook(client, model);
        }

        public static async Task<IReadOnlyList<IWebhook>> FetchChannelWebhooksAsync(this IRestClient client,
            Snowflake channelId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchChannelWebhooksAsync(channelId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, static (x, client) => new TransientWebhook(client, x));
        }

        public static async Task<IReadOnlyList<IWebhook>> FetchGuildWebhooksAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildWebhooksAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList(client, static (x, client) => new TransientWebhook(client, x));
        }

        public static async Task<IWebhook> FetchWebhookAsync(this IRestClient client,
            Snowflake webhookId, string token = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var model = await client.ApiClient.FetchWebhookAsync(webhookId, token, options, cancellationToken).ConfigureAwait(false);
                return new TransientWebhook(client, model);
            }
            catch (RestApiException ex) when (ex.ErrorModel.Code == RestApiErrorCode.UnknownWebhook)
            {
                return null;
            }
        }

        public static async Task<IWebhook> ModifyWebhookAsync(this IRestClient client,
            Snowflake webhookId, Action<ModifyWebhookActionProperties> action, string token = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new ModifyWebhookActionProperties();
            action.Invoke(properties);
            var content = new ModifyWebhookJsonRestRequestContent
            {
                Name = properties.Name,
                Avatar = properties.Avatar,
                ChannelId = properties.ChannelId
            };

            var model = await client.ApiClient.ModifyWebhookAsync(webhookId, content, token, options, cancellationToken).ConfigureAwait(false);
            return new TransientWebhook(client, model);
        }

        public static Task DeleteWebhookAsync(this IRestClient client,
            Snowflake webhookId, string token = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteWebhookAsync(webhookId, token, options, cancellationToken);
        }

        public static async Task<IUserMessage> ExecuteWebhookAsync(this IRestClient client,
            Snowflake webhookId, string token, LocalWebhookMessage message,
            Snowflake? threadId = null, bool wait = false,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            Guard.IsNotNull(message);

            var messageContent = new ExecuteWebhookJsonRestRequestContent
            {
                Content = Optional.FromNullable(message.Content),
                Username = Optional.FromNullable(message.Name),
                AvatarUrl = Optional.FromNullable(message.AvatarUrl),
                Tts = Optional.Conditional(message.IsTextToSpeech, true),
                Embeds = Optional.Conditional(message.Embeds.Count != 0, x => x.Select(x => x.ToModel()).ToArray(), message.Embeds),
                AllowedMentions = Optional.FromNullable(message.AllowedMentions.ToModel()),
                Components = Optional.Conditional(message.Components.Count != 0, x => x.Select(x => x.ToModel()).ToArray(), message.Components)
            };

            Task<MessageJsonModel> task;
            if (message._attachments.Count != 0)
            {
                // If there is an attachment, we must send it via multipart HTTP content.
                // Our `messageContent` will be serialized into a "payload_json" form data field.
                var content = new MultipartJsonPayloadRestRequestContent<ExecuteWebhookJsonRestRequestContent>(messageContent, message._attachments);
                task = client.ApiClient.ExecuteWebhookAsync(webhookId, token, content, threadId, wait, options, cancellationToken);
            }
            else
            {
                task = client.ApiClient.ExecuteWebhookAsync(webhookId, token, messageContent, threadId, wait, options, cancellationToken);
            }

            // If the `wait` parameter is false the model will be null.
            var model = await task.ConfigureAwait(false);
            if (model == null)
                return null;

            return new TransientUserMessage(client, model);
        }

        public static async Task<IUserMessage> FetchWebhookMessageAsync(this IRestClient client,
            Snowflake webhookId, string token, Snowflake messageId,
            Snowflake? threadId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchWebhookMessageAsync(webhookId, token, messageId, threadId, options, cancellationToken).ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static async Task<IUserMessage> ModifyWebhookMessageAsync(this IRestClient client,
            Snowflake webhookId, string token, Snowflake messageId, Action<ModifyWebhookMessageActionProperties> action,
            Snowflake? threadId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new ModifyWebhookMessageActionProperties();
            action.Invoke(properties);
            var messageContent = new ModifyWebhookMessageJsonRestRequestContent
            {
                Content = properties.Content,
                Embeds = Optional.Convert(properties.Embeds, x => x.Select(x => x.ToModel()).ToArray()),
                AllowedMentions = Optional.Convert(properties.AllowedMentions, x => x.ToModel()),
                Attachments = Optional.Convert(properties.AttachmentIds, x => x.Select(x => new AttachmentJsonModel
                {
                    Id = x
                }).ToArray())
            };

            Task<MessageJsonModel> task;
            if (properties.Attachments.HasValue)
            {
                // If there is an attachment, we must send it via multipart HTTP content.
                // Our `messageContent` will be serialized into a "payload_json" form data field.
                var content = new MultipartJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent>(messageContent, properties.Attachments.Value);
                task = client.ApiClient.ModifyWebhookMessageAsync(webhookId, token, messageId, content, threadId, options, cancellationToken);
            }
            else
            {
                task = client.ApiClient.ModifyWebhookMessageAsync(webhookId, token, messageId, messageContent, threadId, options, cancellationToken);
            }

            var model = await task.ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static Task DeleteWebhookMessageAsync(this IRestClient client,
            Snowflake webhookId, string token, Snowflake messageId,
            Snowflake? threadId = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteWebhookMessageAsync(webhookId, token, messageId, threadId, options, cancellationToken);
        }
    }
}
