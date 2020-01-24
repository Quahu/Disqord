using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed class RestWebhookClient : IDisposable
    {
        public static readonly Regex WebhookUrlRegex = new Regex(
            @"discordapp.com/api/webhooks/(?:(?<id>\d+)/(?<token>.+))",
            RegexOptions.Compiled | RegexOptions.ECMAScript);

        public Snowflake Id { get; }

        public string Token { get; }

        public RestDownloadable<RestWebhook> Webhook { get; }

        private readonly RestDiscordClient _client;

        public RestWebhookClient(RestWebhook webhook)
            : this((webhook ?? throw new ArgumentNullException(nameof(webhook))).Client, webhook.Id, webhook.Token)
        {
            Webhook.SetValue(webhook);
        }

        public RestWebhookClient(Snowflake id, string token)
            : this(RestDiscordClient.CreateWithoutAuthorization(), id, token)
        { }

        public RestWebhookClient(RestDiscordClient client, Snowflake id, string token)
            : this(client ?? throw new ArgumentNullException(nameof(client)))
        {
            if (token == null)
                throw new ArgumentNullException(nameof(token));

            Id = id;
            Token = token;
        }

        private RestWebhookClient(RestDiscordClient client)
        {
            _client = client;
            Webhook = new RestDownloadable<RestWebhook>(options => _client.GetWebhookAsync(Id, Token, options));
        }

        public Task<RestUserMessage> ExecuteAsync(string content = null, bool textToSpeech = false, params LocalEmbed[] embeds)
            => _client.ExecuteWebhookAsync(Id, Token, content, textToSpeech, embeds);

        public Task<RestUserMessage> ExecuteAsync(string content = null, bool textToSpeech = false, IEnumerable<LocalEmbed> embeds = null,
            string name = null, string avatarUrl = null, bool wait = false,
            RestRequestOptions options = null)
            => _client.ExecuteWebhookAsync(Id, Token, content, textToSpeech, embeds, name, avatarUrl, wait, options);

        public Task<RestUserMessage> ExecuteAsync(LocalAttachment attachment, string content = null, bool textToSpeech = false,
            IEnumerable<LocalEmbed> embeds = null, string name = null, string avatarUrl = null, bool wait = false,
            RestRequestOptions options = null)
            => _client.ExecuteWebhookAsync(Id, Token, attachment, content, textToSpeech, embeds, name, avatarUrl, wait, options);

        public Task<RestUserMessage> ExecuteAsync(IEnumerable<LocalAttachment> attachments, string content = null, bool textToSpeech = false,
            IEnumerable<LocalEmbed> embeds = null, string name = null, string avatarUrl = null, bool wait = false,
            RestRequestOptions options = null)
            => _client.ExecuteWebhookAsync(Id, Token, attachments, content, textToSpeech, embeds, name, avatarUrl, wait, options);

        public Task<RestWebhook> GetWebhookAsync(RestRequestOptions options = null)
            => Webhook.DownloadAsync(options);

        public async Task<RestWebhook> ModifyAsync(Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            if (Webhook.HasValue)
            {
                await Webhook.Value.ModifyAsync(action, options).ConfigureAwait(false);
                return Webhook.Value;
            }
            else
            {
                var webhook = await _client.ModifyWebhookAsync(Id, action, options).ConfigureAwait(false);
                Webhook.SetValue(webhook);
                return webhook;
            }
        }

        public async Task<RestWebhook> ModifyWithTokenAsync(Action<ModifyWebhookProperties> action, RestRequestOptions options = null)
        {
            if (Webhook.HasValue)
            {
                await Webhook.Value.ModifyWithTokenAsync(action, options).ConfigureAwait(false);
                return Webhook.Value;
            }
            else
            {
                var webhook = await _client.ModifyWebhookAsync(Id, Token, action, options).ConfigureAwait(false);
                Webhook.SetValue(webhook);
                return webhook;
            }
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => _client.DeleteWebhookAsync(Id, options);

        public Task DeleteWithTokenAsync(RestRequestOptions options = null)
            => _client.DeleteWebhookAsync(Id, Token, options);

        public void Dispose()
        {
            _client.Dispose();
        }

        public static RestWebhookClient FromUrl(string url)
        {
            var match = WebhookUrlRegex.Match(url);
            if (!match.Success)
                throw new FormatException("The provided webhook url was not in the correct format.");

            var id = ulong.Parse(match.Groups["id"].Value);
            var token = match.Groups["token"].Value;
            return new RestWebhookClient(id, token);
        }
    }
}
