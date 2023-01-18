using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Qommon.Binding;

namespace Disqord.Rest.Api;

/// <summary>
///     Represents the type responsible for delaying REST requests to prevent exceeding rate-limits.
/// </summary>
public interface IRestRateLimiter : IBindable<IRestApiClient>, ILogging
{
    /// <summary>
    ///     Gets the REST API client of this rate-limiter.
    /// </summary>
    IRestApiClient ApiClient { get; }

    /// <summary>
    ///     Gets whether the given route is currently rate-limited.
    ///     If <paramref name="route"/> is <see langword="null"/>, checks the global rate-limit.
    /// </summary>
    /// <param name="route"> The route to check. </param>
    /// <returns>
    ///     <see langword="true"/> if rate-limited.
    /// </returns>
    bool IsRateLimited(IFormattedRoute? route = null);

    /// <summary>
    ///     Executes the specified request while handling rate-limits.
    /// </summary>
    /// <param name="request"> The request to execute. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task{TResult}"/> representing the work
    ///     with the result being the REST response.
    /// </returns>
    Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken);
}
