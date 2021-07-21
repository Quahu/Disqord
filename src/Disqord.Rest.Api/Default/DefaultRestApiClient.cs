using System.IO;
using System.Text;
using System.Threading.Tasks;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Rest.Api.Default
{
    public class DefaultRestApiClient : IRestApiClient
    {
        /// <inheritdoc/>
        public Token Token { get; }

        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <inheritdoc/>
        public IRestRateLimiter RateLimiter { get; }

        /// <inheritdoc/>
        public IRestRequester Requester { get; }

        /// <inheritdoc/>
        public IJsonSerializer Serializer { get; }

        public DefaultRestApiClient(
            IOptions<DefaultRestApiClientConfiguration> options,
            ILogger<DefaultRestApiClient> logger,
            Token token,
            IRestRateLimiter rateLimiter,
            IRestRequester requester,
            IJsonSerializer serializer)
        {
            Logger = logger;
            Token = token;
            RateLimiter = rateLimiter;
            RateLimiter.Bind(this);
            Requester = requester;
            Requester.Bind(this);
            Serializer = serializer;
        }

        /// <inheritdoc/>
        public async Task ExecuteAsync(FormattedRoute route, IRestRequestContent content = null, IRestRequestOptions options = null)
        {
            await InternalExecuteAsync(route, content, options ?? new DefaultRestRequestOptions()).ConfigureAwait(false);
        }

        public async Task<TModel> ExecuteAsync<TModel>(FormattedRoute route, IRestRequestContent content = null, IRestRequestOptions options = null)
            where TModel : class
        {
            await using (var jsonStream = await InternalExecuteAsync(route, content, options).ConfigureAwait(false))
            {
                if (typeof(TModel) == typeof(string))
                {
                    var reader = new StreamReader(jsonStream, Encoding.UTF8);
                    return (TModel) (object) await reader.ReadToEndAsync().ConfigureAwait(false);
                }

                return Serializer.Deserialize<TModel>(jsonStream);
            }
        }

        private static bool IsValidResponse(IRestResponse response)
            => (int) response.HttpResponse.Code > 199 && (int) response.HttpResponse.Code < 300;

        private async ValueTask<Stream> InternalExecuteAsync(FormattedRoute route, IRestRequestContent content, IRestRequestOptions options)
        {
            var request = new DefaultRestRequest(route, content, options);
            var defaultOptions = options as DefaultRestRequestOptions;
            defaultOptions?.RequestAction?.Invoke(request);
            await RateLimiter.EnqueueRequestAsync(request).ConfigureAwait(false);
            var response = await request.WaitAsync().ConfigureAwait(false);
            defaultOptions?.HeadersAction?.Invoke(new DefaultRestResponseHeaders(response.HttpResponse.Headers));
            var jsonStream = await response.HttpResponse.ReadAsync().ConfigureAwait(false);
            if (IsValidResponse(response))
                return jsonStream;

            await using (jsonStream)
            {
                var errorModel = Serializer.Deserialize<RestApiErrorJsonModel>(jsonStream);
                throw new RestApiException(response.HttpResponse.Code, errorModel);
            }
        }
    }
}
