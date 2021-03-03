using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Disqord.Rest.Default;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Rest.Api.Default
{
    public class DefaultRestApiClient : IRestApiClient
    {
        public Token Token { get; }

        public ILogger Logger { get; }

        public IRestRateLimiter RateLimiter { get; }

        public IRestRequester Requester { get; }

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

        public async Task<IRestResponse> ExecuteAsync(IRestRequest request)
        {
            await RateLimiter.EnqueueRequestAsync(request).ConfigureAwait(false);
            return await request.WaitAsync().ConfigureAwait(false);
        }

        public async Task ExecuteAsync(FormattedRoute route, IRestRequestContent content = null, IRestRequestOptions options = null)
        {
            await InternalExecuteAsync(route, content, options ?? new DefaultRestRequestOptions()).ConfigureAwait(false);
        }

        public async Task<TModel> ExecuteAsync<TModel>(FormattedRoute route, IRestRequestContent content = null, IRestRequestOptions options = null) where TModel : class
        {
            var buffer = await InternalExecuteAsync(route, content, options ?? new DefaultRestRequestOptions()).ConfigureAwait(false);
            if (typeof(TModel) == typeof(string))
                return (TModel) (object) Encoding.UTF8.GetString(buffer);

            var model = Serializer.Deserialize<TModel>(buffer);
            if (model is JsonModel jsonModel && jsonModel.ExtensionData != null && jsonModel.ExtensionData.Count > 0 && jsonModel.GetType() != typeof(JsonModel))
            {
                Logger.LogTrace("Found {0} extra fields for model {1}:\n{2}", jsonModel.ExtensionData.Count, jsonModel, string.Join('\n', jsonModel.ExtensionData));
            }

            return model;
        }

        private static bool IsValidResponse(IRestResponse response)
            => (int) response.HttpResponse.Code > 199 && (int) response.HttpResponse.Code < 300;

        private async Task<ArraySegment<byte>> InternalExecuteAsync(FormattedRoute route, IRestRequestContent content, IRestRequestOptions options)
        {
            var request = new DefaultRestRequest(route, content, options);
            if (options is DefaultRestRequestOptions defaultOptions)
            {
                defaultOptions.RequestAction?.Invoke(request);
            }

            await RateLimiter.EnqueueRequestAsync(request).ConfigureAwait(false);
            var response = await request.WaitAsync().ConfigureAwait(false);
            using (var jsonStream = await response.HttpResponse.ReadAsync().ConfigureAwait(false))
            {
                var stream = new MemoryStream((int) jsonStream.Length);
                await jsonStream.CopyToAsync(stream).ConfigureAwait(false);
                stream.TryGetBuffer(out var buffer);

                if (!IsValidResponse(response))
                {
                    var errorModel = Serializer.Deserialize<RestApiErrorJsonModel>(buffer);
                    throw new RestApiException(response.HttpResponse.Code, errorModel);
                }

                return buffer;
            }
        }

        public void Dispose()
        {
            RateLimiter.Dispose();
            Requester.Dispose();
            Serializer.Dispose();
        }
    }
}
