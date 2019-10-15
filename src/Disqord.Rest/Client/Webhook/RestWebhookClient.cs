using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    public sealed class RestWebhookClient : IDisposable
    {
        internal static readonly Regex UrlRegex = new Regex(@"discordapp.com/api/webhooks/(?:(?<id>\d+)/(?<token>.+))", RegexOptions.Compiled | RegexOptions.ECMAScript);

        public Snowflake Id { get; }

        public string Token { get; }

        public RestDownloadable<RestWebhook> Webhook { get; }

        private readonly RestDiscordClient _client;

        public RestWebhookClient(RestWebhook webhook, ILogger logger = null, IJsonSerializer serializer = null) : this(logger, serializer)
        {
            if (webhook == null)
                throw new ArgumentNullException(nameof(webhook));

            Id = webhook.Id;
            Token = webhook.Token;
            Webhook.SetValue(webhook);
        }

        public RestWebhookClient(ulong id, string token, ILogger logger = null, IJsonSerializer serializer = null) : this(logger, serializer)
        {
            Id = id;
            Token = token;
        }

        public RestWebhookClient(string url, ILogger logger = null, IJsonSerializer serializer = null) : this(logger, serializer)
        {
            var match = UrlRegex.Match(url);
            if (!match.Success)
                throw new ArgumentException("The provided webhook url was not the correct format.", nameof(url));

            Id = ulong.Parse(match.Groups["id"].Value);
            Token = match.Groups["token"].Value;
        }

        private RestWebhookClient(ILogger logger, IJsonSerializer serializer)
        {
            Webhook = new RestDownloadable<RestWebhook>(options => _client.GetWebhookWithTokenAsync(Id, Token, options));
            _client = RestDiscordClient.CreateWithoutAuthorization(logger, serializer);
        }

        public Task<RestUserMessage> ExecuteAsync(string content = null, bool textToSpeech = false, params Embed[] embeds)
            => _client.ExecuteWebhookAsync(Id, Token, content, textToSpeech, embeds);

        public Task<RestUserMessage> ExecuteAsync(string content = null, bool textToSpeech = false, IEnumerable<Embed> embeds = null, string name = null, string avatarUrl = null, bool wait = false)
            => _client.ExecuteWebhookAsync(Id, Token, content, textToSpeech, embeds, name, avatarUrl, wait);

        public Task<RestUserMessage> ExecuteAsync(LocalAttachment attachment, string content = null, bool textToSpeech = false, params Embed[] embeds)
            => _client.ExecuteWebhookAsync(Id, Token, attachment, content, textToSpeech, embeds);

        public Task<RestUserMessage> ExecuteAsync(LocalAttachment attachment, string content = null, bool textToSpeech = false, IEnumerable<Embed> embeds = null, string name = null, string avatarUrl = null, bool wait = false)
            => _client.ExecuteWebhookAsync(Id, Token, attachment, content, textToSpeech, embeds, name, avatarUrl, wait);

        public Task<RestWebhook> GetWebhookAsync()
            => Webhook.DownloadAsync();

        public async Task<RestWebhook> ModifyAsync(Action<ModifyWebhookProperties> action)
        {
            if (Webhook.HasValue)
            {
                await Webhook.Value.ModifyAsync(action).ConfigureAwait(false);
                return Webhook.Value;
            }
            else
            {
                var webhook = await _client.ModifyWebhookWithTokenAsync(Id, Token, action).ConfigureAwait(false);
                Webhook.SetValue(webhook);
                return webhook;
            }
        }

        public Task DeleteAsync()
            => _client.DeleteWebhookWithTokenAsync(Id, Token);

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
