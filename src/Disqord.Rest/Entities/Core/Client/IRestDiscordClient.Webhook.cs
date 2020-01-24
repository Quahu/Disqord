using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<RestWebhook> CreateWebhookAsync(Snowflake channelId, string name, Stream avatar = null, RestRequestOptions options = null);

        Task<IReadOnlyList<RestWebhook>> GetChannelWebhooksAsync(Snowflake channelId, RestRequestOptions options = null);

        Task<IReadOnlyList<RestWebhook>> GetGuildWebhooksAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestWebhook> GetWebhookAsync(Snowflake webhookId, RestRequestOptions options = null);

        Task<RestWebhook> GetWebhookAsync(Snowflake webhookId, string webhookToken, RestRequestOptions options = null);

        Task<RestWebhook> ModifyWebhookAsync(Snowflake webhookId, Action<ModifyWebhookProperties> action, RestRequestOptions options = null);

        Task<RestWebhook> ModifyWebhookAsync(Snowflake webhookId, string webhookToken, Action<ModifyWebhookProperties> action, RestRequestOptions options = null);

        Task DeleteWebhookAsync(Snowflake webhookId, RestRequestOptions options = null);

        Task DeleteWebhookAsync(Snowflake webhookId, string webhookToken, RestRequestOptions options = null);

        Task<RestUserMessage> ExecuteWebhookAsync(Snowflake webhookId, string webhookToken,
            string content = null, bool textToSpeech = false, IEnumerable<LocalEmbed> embeds = null,
            string name = null, string avatarUrl = null,
            bool wait = false,
            RestRequestOptions options = null);

        Task<RestUserMessage> ExecuteWebhookAsync(Snowflake webhookId, string webhookToken,
            LocalAttachment attachment,
            string content = null, bool textToSpeech = false, IEnumerable<LocalEmbed> embeds = null,
            string name = null, string avatarUrl = null,
            bool wait = false,
            RestRequestOptions options = null);

        Task<RestUserMessage> ExecuteWebhookAsync(Snowflake webhookId, string webhookToken,
            IEnumerable<LocalAttachment> attachments,
            string content = null, bool textToSpeech = false, IEnumerable<LocalEmbed> embeds = null,
            string name = null, string avatarUrl = null,
            bool wait = false,
            RestRequestOptions options = null);

        // TODO: execute compatible webhooks (github, slack)
    }
}
