using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Http;
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

    /// <summary>
    ///     Executes the specified route, discarding the response body.
    /// </summary>
    /// <param name="route"> The route to execute. </param>
    /// <param name="content"> The request content. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the work.
    /// </returns>
    Task ExecuteAsync(IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Executes the specified route, deserializing the response body into <typeparamref name="TModel"/>.
    /// </summary>
    /// <remarks>
    ///     Discord answers some routes with <see cref="HttpResponseStatusCode.NoContent"/>,
    ///     either always or depending on the request, in which case the result is <see langword="null"/>.
    ///     Such routes must be declared as returning a nullable model.
    /// </remarks>
    /// <param name="route"> The route to execute. </param>
    /// <param name="content"> The request content. </param>
    /// <param name="options"> The request options. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <typeparam name="TModel"> The type to deserialize the response body into. </typeparam>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the work with the result being the deserialized model.
    /// </returns>
    Task<TModel> ExecuteAsync<TModel>(IFormattedRoute route,
        IRestRequestContent? content = null,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        where TModel : class;
}
