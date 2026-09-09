using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Rest.Api.Default;

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
    public async Task ExecuteAsync(IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var stream = await InternalExecuteAsync(route, content, options, cancellationToken).ConfigureAwait(false);
        await stream.DisposeAsync().ConfigureAwait(false);
    }

    public async Task<TModel> ExecuteAsync<TModel>(IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        where TModel : class
    {
        var jsonStream = await InternalExecuteAsync(route, content, options, cancellationToken).ConfigureAwait(false);
        try
        {
            if (typeof(TModel) == typeof(string))
            {
                var reader = new StreamReader(jsonStream, Encoding.UTF8);
                return Unsafe.As<TModel>(await reader.ReadToEndAsync().ConfigureAwait(false));
            }

            return Serializer.Deserialize<TModel>(jsonStream)!;
        }
        finally
        {
            await jsonStream.DisposeAsync().ConfigureAwait(false);
        }
    }

    private async ValueTask<Stream> InternalExecuteAsync(IFormattedRoute route,
        IRestRequestContent? content,
        IRestRequestOptions? options,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        content?.Validate();

        using (var request = new DefaultRestRequest(route, content, options))
        {
            var defaultOptions = options as DefaultRestRequestOptions;
            defaultOptions?.RequestAction?.Invoke(request);

            var response = await RateLimiter.ExecuteAsync(request, cancellationToken).ConfigureAwait(false);
            defaultOptions?.HeadersAction?.Invoke(new DefaultRestResponseHeaders(response.HttpResponse.Headers));

            var responseStream = await response.HttpResponse.ReadAsync(cancellationToken).ConfigureAwait(false);

            if (response.HttpResponse.StatusCode.IsSuccess())
                return responseStream;

            if (response.HttpResponse.StatusCode.IsServerError())
                throw new RestApiException(response.HttpResponse, response.HttpResponse.ReasonPhrase, null);

            RestApiErrorJsonModel? errorModel = null;
            try
            {
                errorModel = Serializer.Deserialize<RestApiErrorJsonModel>(responseStream);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "An exception occurred while attempting to deserialize the error model.");
            }
            finally
            {
                await responseStream.DisposeAsync().ConfigureAwait(false);
            }

            throw new RestApiException(response.HttpResponse, response.HttpResponse.ReasonPhrase, errorModel);
        }
    }
}
