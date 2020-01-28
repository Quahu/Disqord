using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<RestWebhook> CreateWebhookAsync(Snowflake channelId, string name, Stream avatar = null, RestRequestOptions options = null)
        {
            var model = await ApiClient.CreateWebhookAsync(channelId, name, avatar, options).ConfigureAwait(false);
            return new RestWebhook(this, model);
        }

        public async Task<IReadOnlyList<RestWebhook>> GetChannelWebhooksAsync(Snowflake channelId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetChannelWebhooksAsync(channelId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestWebhook(@this, x));
        }

        public async Task<IReadOnlyList<RestWebhook>> GetGuildWebhooksAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.GetGuildWebhooksAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(this, (x, @this) => new RestWebhook(@this, x));
        }

        public async Task<RestWebhook> GetWebhookAsync(Snowflake webhookId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetWebhookAsync(webhookId, options).ConfigureAwait(false);
                return new RestWebhook(this, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<RestWebhook> GetWebhookAsync(Snowflake webhookId, string webhookToken, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetWebhookWithTokenAsync(webhookId, webhookToken, options).ConfigureAwait(false);
                return new RestWebhook(this, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<RestWebhook> ModifyWebhookAsync(Snowflake webhookId, Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyWebhookAsync(webhookId, action, options).ConfigureAwait(false);
            return new RestWebhook(this, model);
        }

        internal async Task<WebhookModel> InternalModifyWebhookAsync(Snowflake webhookId, Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            var properties = new ModifyWebhookProperties();
            action(properties);
            return await ApiClient.ModifyWebhookAsync(webhookId, properties, options).ConfigureAwait(false);
        }

        public async Task<RestWebhook> ModifyWebhookAsync(Snowflake webhookId, string webhookToken, Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyWebhookAsync(webhookId, webhookToken, action, options).ConfigureAwait(false);
            return new RestWebhook(this, model);
        }

        internal async Task<WebhookModel> InternalModifyWebhookAsync(Snowflake webhookId, string webhookToken, Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            var properties = new ModifyWebhookProperties();
            action(properties);
            return await ApiClient.ModifyWebhookWithTokenAsync(webhookId, webhookToken, properties, options).ConfigureAwait(false);
        }

        public Task DeleteWebhookAsync(Snowflake webhookId, RestRequestOptions options = null)
            => ApiClient.DeleteWebhookAsync(webhookId, options);

        public Task DeleteWebhookAsync(Snowflake webhookId, string webhookToken, RestRequestOptions options = null)
            => ApiClient.DeleteWebhookWithTokenAsync(webhookId, webhookToken, options);

        public async Task<RestUserMessage> ExecuteWebhookAsync(Snowflake webhookId, string webhookToken,
            string content = null, bool textToSpeech = false, IEnumerable<LocalEmbed> embeds = null,
            string name = null, string avatarUrl = null,
            bool wait = false,
            RestRequestOptions options = null)
        {
            var model = await ApiClient.ExecuteWebhookAsync(webhookId, webhookToken, content, textToSpeech, embeds, name, avatarUrl, wait, options).ConfigureAwait(false);
            return wait
                ? new RestUserMessage(this, model)
                : null;
        }

        public async Task<RestUserMessage> ExecuteWebhookAsync(Snowflake webhookId, string webhookToken,
            LocalAttachment attachment,
            string content = null, bool textToSpeech = false, IEnumerable<LocalEmbed> embeds = null,
            string name = null, string avatarUrl = null,
            bool wait = false,
            RestRequestOptions options = null)
        {
            if (attachment == null)
                throw new ArgumentNullException(nameof(attachment));

            var model = await ApiClient.ExecuteWebhookAsync(webhookId, webhookToken, attachment, content, textToSpeech, embeds, name, avatarUrl, wait, options).ConfigureAwait(false);
            return wait
                ? new RestUserMessage(this, model)
                : null;
        }

        public async Task<RestUserMessage> ExecuteWebhookAsync(Snowflake webhookId, string webhookToken,
            IEnumerable<LocalAttachment> attachments,
            string content = null, bool textToSpeech = false, IEnumerable<LocalEmbed> embeds = null,
            string name = null, string avatarUrl = null,
            bool wait = false,
            RestRequestOptions options = null)
        {
            if (attachments == null)
                throw new ArgumentNullException(nameof(attachments));

            var model = await ApiClient.ExecuteWebhookAsync(webhookId, webhookToken, attachments, content, textToSpeech, embeds, name, avatarUrl, wait, options).ConfigureAwait(false);
            return wait ?
                new RestUserMessage(this, model)
                : null;
        }
    }
}
