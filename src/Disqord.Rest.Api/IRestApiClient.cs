using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents a low-level client for the Discord REST API.
/// </summary>
/// <remarks>
///     <inheritdoc/>
/// </remarks>
public interface IRestApiClient : IApiClient
{
    IRestRateLimiter RateLimiter { get; }

    IRestRequester Requester { get; }

    IJsonSerializer Serializer { get; }

    Task ExecuteAsync(IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default);

    Task<TModel> ExecuteAsync<TModel>(IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        where TModel : class;
}
