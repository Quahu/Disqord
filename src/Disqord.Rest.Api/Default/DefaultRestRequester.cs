using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Http.Default;
using Disqord.Utilities.Binding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Rest.Api.Default
{
    public class DefaultRestRequester : IRestRequester
    {
        public int Version { get; }

        public ILogger Logger { get; }

        public IHttpClient HttpClient { get; }

        public IRestApiClient ApiClient => _binder.Value;

        private readonly Binder<IRestApiClient> _binder;

        public DefaultRestRequester(
            IOptions<DefaultRestRequesterConfiguration> options,
            ILogger<DefaultRestRequester> logger,
            IHttpClient httpClient)
        {
            Version = options.Value.Version;
            Logger = logger;
            HttpClient = httpClient;
            HttpClient.BaseUri = new Uri($"https://discord.com/api/v{Version}/");

            HttpClient.SetDefaultHeader("User-Agent", Library.UserAgent);

            _binder = new Binder<IRestApiClient>(this);
        }

        /// <inheritdoc/>
        public void Bind(IRestApiClient apiClient)
        {
            _binder.Bind(apiClient);

            if (ApiClient.Token != null)
                HttpClient.SetDefaultHeader("Authorization", ApiClient.Token.GetAuthorization());
        }

        /// <inheritdoc/>
        public async Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken = default)
        {
            var method = request.Route.BaseRoute.Method;
            var uri = new Uri(request.Route.Path, UriKind.Relative);
            var content = request.GetOrCreateHttpContent(ApiClient);
            var httpRequest = new DefaultHttpRequest(method, uri, content);
            if (request.Options?.Headers != null)
            {
                foreach (var header in request.Options.Headers)
                    httpRequest.Headers.Add(header.Key, header.Value);
            }

            var response = await HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
            return new DefaultRestResponse(response);
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
