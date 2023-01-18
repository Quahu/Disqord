using Disqord.Http;
using Disqord.Serialization.Json;
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

    public DefaultRestRequest(
        IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null)
    {
        Guard.IsNotNull(route);

        Route = route;
        Content = content;
        Options = options;
    }

    public virtual HttpRequestContent? GetOrCreateHttpContent(IJsonSerializer serializer)
    {
        if (HttpContent == null && Content != null)
            HttpContent = Content.CreateHttpContent(serializer, Options);

        return HttpContent;
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        HttpContent?.Dispose();
    }
}
