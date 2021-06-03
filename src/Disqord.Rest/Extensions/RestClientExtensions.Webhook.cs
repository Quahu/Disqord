using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IWebhook> CreateWebhookAsync(this IRestClient client, Snowflake channelId, string name, Action<CreateWebhookActionProperties> action = null, IRestRequestOptions options = null)
        {
            var properties = new CreateWebhookActionProperties();
            action?.Invoke(properties);
            var content = new CreateWebhookJsonRestRequestContent(name)
            {
                Avatar = properties.Avatar
            };
            var model = await client.ApiClient.CreateWebhookAsync(channelId, content, options).ConfigureAwait(false);
            return new TransientWebhook(client, model);
        }

        public static async Task<IReadOnlyList<IWebhook>> FetchChannelWebhooksAsync(this IRestClient client, Snowflake channelId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchChannelWebhooksAsync(channelId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, static(x, client) => new TransientWebhook(client, x));
        }

        public static async Task<IReadOnlyList<IWebhook>> FetchGuildWebhooksAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildWebhooksAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, static(x, client) => new TransientWebhook(client, x));
        }

        public static async Task<IWebhook> FetchWebhookAsync(this IRestClient client, Snowflake webhookId, string token = null, IRestRequestOptions options = null)
        {
            try
            {
                var model = await client.ApiClient.FetchWebhookAsync(webhookId, token, options).ConfigureAwait(false);
                return new TransientWebhook(client, model);
            }
            catch (RestApiException ex) when (ex.ErrorModel.Code == RestApiErrorCode.UnknownWebhook)
            {
                return null;
            }
        }

        public static async Task<IWebhook> ModifyWebhookAsync(this IRestClient client, Snowflake webhookId, Action<ModifyWebhookActionProperties> action, string token = null, IRestRequestOptions options = null)
        {
            var properties = new ModifyWebhookActionProperties();
            action.Invoke(properties);
            var content = new ModifyWebhookJsonRestRequestContent
            {
                Name = properties.Name,
                Avatar = properties.Avatar,
                ChannelId = properties.ChannelId
            };
            var model = await client.ApiClient.ModifyWebhookAsync(webhookId, content, token, options).ConfigureAwait(false);
            return new TransientWebhook(client, model);
        }

        public static Task DeleteWebhookAsync(this IRestClient client, Snowflake webhookId, string token = null, IRestRequestOptions options = null)
            => client.ApiClient.DeleteWebhookAsync(webhookId, token, options);

        public static async Task<IUserMessage> ExecuteWebhookAsync(this IRestClient client, Snowflake webhookId, string token, LocalWebhookMessage message, bool wait = false, IRestRequestOptions options = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var messageContent = new ExecuteWebhookJsonRestRequestContent
            {
                Content = Optional.FromNullable(message.Content),
                Username = Optional.FromNullable(message.Name),
                AvatarUrl = Optional.FromNullable(message.AvatarUrl),
                Tts = Optional.Conditional(message.IsTextToSpeech, true),
                Embeds = Optional.Conditional(message.Embeds.Count != 0, x => x.Select(x => x.ToModel()).ToArray(), message.Embeds),
                AllowedMentions = Optional.FromNullable(message.AllowedMentions.ToModel()),
            };

            Task<MessageJsonModel> task;
            if (message.Attachment != null)
            {
                // If there is an attachment, we must send it via multipart HTTP content.
                // Our `messageContent` will be serialized into a "payload_json" form data field.
                var content = new MultipartJsonPayloadRestRequestContent<ExecuteWebhookJsonRestRequestContent>(messageContent, new[] { message.Attachment });
                task = client.ApiClient.ExecuteWebhookAsync(webhookId, token, content, wait, options);
            }
            else
            {
                task = client.ApiClient.ExecuteWebhookAsync(webhookId, token, messageContent, wait, options);
            }

            // If the `wait` parameter is false the model will be null.
            var model = await task.ConfigureAwait(false);
            if (model == null)
                return null;

            return new TransientUserMessage(client, model);
        }

        public static async Task<IUserMessage> FetchWebhookMessageAsync(this IRestClient client, Snowflake webhookId, string token, Snowflake messageId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchWebhookMessageAsync(webhookId, token, messageId, options).ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static async Task<IUserMessage> ModifyWebhookMessageAsync(this IRestClient client, Snowflake webhookId, string token, Snowflake messageId, Action<ModifyWebhookMessageActionProperties> action, IRestRequestOptions options = null)
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
            if (properties.Attachment.HasValue)
            {
                // TODO: delete files?
                if (properties.Attachment.Value == null)
                    throw new InvalidOperationException("The attachment must not be null.");

                // If there is an attachment, we must send it via multipart HTTP content.
                // Our `messageContent` will be serialized into a "payload_json" form data field.
                var content = new MultipartJsonPayloadRestRequestContent<ModifyWebhookMessageJsonRestRequestContent>(messageContent, new[] { properties.Attachment.Value });
                task = client.ApiClient.ModifyWebhookMessageAsync(webhookId, token, messageId, content, options);
            }
            else
            {
                task = client.ApiClient.ModifyWebhookMessageAsync(webhookId, token, messageId, messageContent, options);
            }

            var model = await task.ConfigureAwait(false);
            return new TransientUserMessage(client, model);
        }

        public static Task DeleteWebhookMessageAsync(this IRestClient client, Snowflake webhookId, string token, Snowflake messageId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteWebhookMessageAsync(webhookId, token, messageId, options);
    }
}
