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

        public RestRequestOptions Options { get; }

        private TaskCompletionSource<HttpResponseMessage> _tcs;

        private readonly FormattableString _url;

        private readonly HttpMethod _method;

        private readonly IReadOnlyDictionary<string, object> _queryStringParameters;

        private readonly IRequestContent _content;

        public RestRequest(HttpMethod method, FormattableString url, RestRequestOptions options) : this(method, url, null, null, options)
        { }

        public RestRequest(HttpMethod method, FormattableString url, IRequestContent content, RestRequestOptions options) : this(method, url, null, content, options)
        { }

        public RestRequest(HttpMethod method, FormattableString url, IReadOnlyDictionary<string, object> queryStringParameters, RestRequestOptions options) : this(method, url, queryStringParameters, null, options)
        { }

        public RestRequest(HttpMethod method, FormattableString url, IReadOnlyDictionary<string, object> queryStringParameters, IRequestContent content, RestRequestOptions options)
        {
            _tcs = new TaskCompletionSource<HttpResponseMessage>();
            _url = url;
            _method = method;
            _queryStringParameters = queryStringParameters;
            _content = content;
            Options = options?.Clone();
        }

        public void Initialise(IJsonSerializer serializer)
        {
            HttpMessage?.Dispose();
            var extractedUrl = ExtractUrl(_url, out var route, out var guildId, out var channelId, out var webhookId);
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
            Identifier = RateLimitBucket.GenerateIdentifier(_method, route, guildId, channelId, webhookId);
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

            return string.Concat('?', string.Join("&", parameters.Select(x => string.Concat(x.Key, '=', Uri.EscapeDataString(x.Value.ToString())))));
        }

        internal static string ExtractUrl(FormattableString formattableUrl, out string route, out ulong? guildId, out ulong? channelId, out ulong? webhookId)
        {
            var raw = formattableUrl.Format;
            var arguments = formattableUrl.GetArguments();
            guildId = null;
            channelId = null;
            webhookId = null;

            if (arguments.Length == 0)
            {
                route = raw;
                return raw;
            }

            var urlBuilder = new StringBuilder();
            var routeBuilder = new StringBuilder();

            var argumentPosition = 0;
            var startPosition = 0;
            int firstIndex;
            var secondIndex = -1;
            while ((firstIndex = raw.IndexOf('{', startPosition)) != -1)
            {
                var segment = raw.Substring(startPosition, firstIndex - startPosition);
                urlBuilder.Append(segment);
                routeBuilder.Append(segment);

                startPosition = firstIndex + 1;
                secondIndex = raw.IndexOf('}', startPosition);
                if (secondIndex == -1)
                    throw new Exception();

                startPosition = secondIndex + 1;
                var split = raw.Substring(firstIndex, secondIndex - firstIndex).Split(':');
                if (split.Length != 2)
                    throw new Exception();

                var name = split[1];
                var value = arguments[argumentPosition++];
                switch (name)
                {
                    case "guild_id":
                        guildId = Convert.ToUInt64(value);
                        break;

                    case "channel_id":
                        channelId = Convert.ToUInt64(value);
                        break;

                    case "webhook_id":
                        webhookId = Convert.ToUInt64(value);
                        break;
                }

                routeBuilder.Append(name);
                urlBuilder.Append(value);
            }

            secondIndex++;
            if (secondIndex < raw.Length - 1)
            {
                var rest = raw.Substring(secondIndex);
                if (firstIndex == -1)
                    routeBuilder.Append(rest);
                urlBuilder.Append(rest);
            }

            route = routeBuilder.ToString();
            return urlBuilder.ToString();
        }
    }
}
