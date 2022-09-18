using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Http.Default;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon;
using Qommon.Binding;

namespace Disqord.Rest.Api.Default;

public class DefaultRestRequester : IRestRequester
{
    /// <inheritdoc/>
    public int Version { get; }

    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public IHttpClient HttpClient { get; }

    /// <inheritdoc/>
    public IRestApiClient ApiClient => _binder.Value;

    private readonly Binder<IRestApiClient> _binder;

    public DefaultRestRequester(
        IOptions<DefaultRestRequesterConfiguration> options,
        ILogger<DefaultRestRequester> logger,
        IHttpClientFactory httpClientFactory)
    {
        Version = options.Value.Version;
        Logger = logger;
        HttpClient = httpClientFactory.CreateClient();
        HttpClient.BaseUri = new Uri($"https://discord.com/api/v{Version}/");
        HttpClient.SetDefaultHeader("User-Agent", Library.UserAgent);

        _binder = new Binder<IRestApiClient>(this);
    }

    /// <inheritdoc/>
    public void Bind(IRestApiClient apiClient)
    {
        _binder.Bind(apiClient);

        var authorization = ApiClient.Token.GetAuthorization();
        if (authorization != null)
            HttpClient.SetDefaultHeader("Authorization", authorization);
    }

    /// <inheritdoc/>
    public async Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(request);

        var method = request.Route.BaseRoute.Method;
        var uri = new Uri(request.Route.Path, UriKind.Relative);
        var content = request.GetOrCreateHttpContent(ApiClient.Serializer);
        var httpRequest = new DefaultHttpRequest(method, uri, content);

        if (request.Options?.Headers != null)
        {
            foreach (var header in request.Options.Headers)
                httpRequest.Headers.Add(header.Key, header.Value);
        }

        if (httpRequest.Headers.TryGetValue(RestApiHeaderNames.AuditLogReason, out var auditLogReason))
        {
            // URI-encodes the audit log reason to allow for non-ASCII characters.
            httpRequest.Headers[RestApiHeaderNames.AuditLogReason] = Uri.EscapeDataString(auditLogReason);
        }

        var response = await HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        return new DefaultRestResponse(response);
    }
}
