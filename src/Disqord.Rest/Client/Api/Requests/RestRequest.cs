using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class RestRequest : IDisposable
    {
        public string Identifier { get; private set; }

        public HttpRequestMessage HttpMessage { get; private set; }

        public int? RateLimitOverride { get; set; }

        public bool BucketsMethod { get; set; }

        public RestRequestOptions Options { get; }

        private TaskCompletionSource<HttpResponseMessage> _tcs;

        private readonly FormattableString _url;

        private readonly HttpMethod _method;

        private readonly IReadOnlyDictionary<string, object> _queryStringParameters;

        private readonly IRequestContent _content;

        public RestRequest(HttpMethod method, FormattableString url, IReadOnlyDictionary<string, object> queryStringParameters, IRequestContent content, RestRequestOptions options)
        {
            _tcs = new TaskCompletionSource<HttpResponseMessage>();
            _url = url;
            _method = method;
            _queryStringParameters = queryStringParameters;
            _content = content;
            Options = options.Clone();
        }

        public void Initialise(IJsonSerializer serializer)
        {
            HttpMessage?.Dispose();
            var extractedUrl = FormatUrl(_url, out var guildId, out var channelId, out var webhookId);
            var builtQueryString = BuildQueryString(_queryStringParameters);
            var message = new HttpRequestMessage
            {
                Method = _method,
                RequestUri = new Uri(string.Concat(extractedUrl, builtQueryString), UriKind.Relative),
                Content = _content?.Prepare(serializer, Options)
            };

            message.Headers.Add("X-Ratelimit-Precision", "millisecond");

            var reason = Options?.Reason;
            if (reason != null)
                message.Headers.Add("X-Audit-Log-Reason", Uri.EscapeDataString(reason));

            HttpMessage = message;
            Identifier = RateLimitBucket.GenerateIdentifier(_method, BucketsMethod, _url.Format, guildId, channelId, webhookId);
        }

        public Task<HttpResponseMessage> CompleteAsync()
            => _tcs.Task;

        public void SetResult(HttpResponseMessage httpResponseMessage)
            => _tcs.SetResult(httpResponseMessage);

        public void SetException(Exception exception)
            => _tcs.SetException(exception);

        public void Dispose()
        {
            HttpMessage.Dispose();
            _tcs = null;
        }

        public override string ToString()
            => Identifier;

        internal static string BuildQueryString(IReadOnlyDictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                return null;

            return string.Concat("?", string.Join('&', parameters.Select(x => string.Concat(x.Key, "=", Uri.EscapeDataString(x.Value.ToString())))));
        }

        internal static string FormatUrl(FormattableString formattable, out ulong guildId, out ulong channelId, out ulong webhookId)
        {
            var raw = formattable.Format;
            guildId = 0;
            channelId = 0;
            webhookId = 0;

            if (formattable.ArgumentCount == 0)
                return raw;

            var builder = new StringBuilder(raw.Length);
            var rawSpan = raw.AsSpan();
            int firstBracketIndex;
            while ((firstBracketIndex = rawSpan.IndexOf('{')) != -1)
            {
                builder.Append(rawSpan.Slice(0, firstBracketIndex));
                rawSpan = rawSpan.Slice(firstBracketIndex + 1);
                var secondBracketIndex = rawSpan.IndexOf('}');
                var segment = rawSpan.Slice(0, secondBracketIndex);
                int argumentIndex = segment[0] - '0';
                var argument = formattable.GetArgument(argumentIndex);
                builder.Append(argument);
                if (segment.Length > 1)
                {
                    var nameSpan = segment.Slice(2);
                    if (nameSpan.Equals("guild_id", StringComparison.Ordinal))
                        guildId = Convert.ToUInt64(argument);

                    else if (nameSpan.Equals("channel_id", StringComparison.Ordinal))
                        channelId = Convert.ToUInt64(argument);

                    else if (nameSpan.Equals("webhook_id", StringComparison.Ordinal))
                        webhookId = Convert.ToUInt64(argument);

                    else
                        throw new ArgumentException($"Unrecognized url name '{nameSpan.ToString()}'.", nameof(formattable));
                }

                rawSpan = rawSpan.Slice(secondBracketIndex + 1);
            }

            if (rawSpan.Length > 0)
                builder.Append(rawSpan);

            return builder.ToString();
        }
    }
}
