using System;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Serialization.Json;
using Disqord.Utilities.Threading;
using Qommon;

namespace Disqord.Rest.Api.Default;

/// <inheritdoc/>
public class DefaultRestRequest : IRestRequest
{
    /// <inheritdoc/>
    public IFormattedRoute Route { get; }

    /// <inheritdoc/>
    public IRestRequestContent? Content { get; }

    /// <inheritdoc/>
    public IRestRequestOptions? Options { get; }

    protected HttpRequestContent? HttpContent;

    private readonly Tcs<IRestResponse> _tcs;

    public DefaultRestRequest(IFormattedRoute route, IRestRequestContent? content = null, IRestRequestOptions? options = null)
    {
        Guard.IsNotNull(route);

        Route = route;
        Content = content;
        Options = options;

        _tcs = new Tcs<IRestResponse>();
    }

    public virtual HttpRequestContent? GetOrCreateHttpContent(IJsonSerializer serializer)
    {
        if (HttpContent == null && Content != null)
            HttpContent = Content.CreateHttpContent(serializer, Options);

        return HttpContent;
    }

    /// <inheritdoc/>
    public Task<IRestResponse> WaitForCompletionAsync()
    {
        return _tcs.Task;
    }

    /// <inheritdoc/>
    public void Complete(IRestResponse response)
    {
        Guard.IsNotNull(response);

        _tcs.Complete(response);
    }

    /// <inheritdoc/>
    public void Complete(Exception exception)
    {
        Guard.IsNotNull(exception);

        _tcs.Throw(exception);
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        HttpContent?.Dispose();
    }
}
