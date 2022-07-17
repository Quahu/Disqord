using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        var stream = await InternalExecuteAsync(route, content, options ?? new DefaultRestRequestOptions()).ConfigureAwait(false);
        await stream.DisposeAsync().ConfigureAwait(false);
    }

    public async Task<TModel> ExecuteAsync<TModel>(IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        where TModel : class
    {
        await using (var jsonStream = await InternalExecuteAsync(route, content, options).ConfigureAwait(false))
        {
            if (typeof(TModel) == typeof(string))
            {
                var reader = new StreamReader(jsonStream, Encoding.UTF8);
                return Unsafe.As<TModel>(await reader.ReadToEndAsync().ConfigureAwait(false));
            }

            return Serializer.Deserialize<TModel>(jsonStream)!;
        }
    }

    private async ValueTask<Stream> InternalExecuteAsync(IFormattedRoute route,
        IRestRequestContent? content,
        IRestRequestOptions? options)
    {
        content?.Validate();

        var request = new DefaultRestRequest(route, content, options);

        var defaultOptions = options as DefaultRestRequestOptions;
        defaultOptions?.RequestAction?.Invoke(request);

        await RateLimiter.EnqueueRequestAsync(request).ConfigureAwait(false);

        var response = await request.WaitForCompletionAsync().ConfigureAwait(false);
        defaultOptions?.HeadersAction?.Invoke(new DefaultRestResponseHeaders(response.HttpResponse.Headers));

        var responseStream = await response.HttpResponse.ReadAsync().ConfigureAwait(false);

        var statusCode = (int) response.HttpResponse.StatusCode;
        if (statusCode > 199 && statusCode < 300)
            return responseStream;

        if (statusCode > 499 && statusCode < 600)
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
