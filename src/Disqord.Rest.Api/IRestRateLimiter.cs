using System;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Utilities.Binding;

namespace Disqord.Rest.Api
{
    public interface IRestRateLimiter : IBindable<IRestApiClient>, ILogging, IDisposable
    {
        IRestApiClient ApiClient { get; }

        /// <summary>
        ///     Gets whether the given route is currently rate-limited.
        ///     If given <see langword="null"/>, checks the global rate-limit.
        /// </summary>
        /// <param name="route"> The route to check. </param>
        /// <returns>
        ///     <see langword="true"/> if rate-limited.
        /// </returns>
        bool IsRateLimited(FormattedRoute route = null);

        /// <summary>
        ///     Enqueues the specified request for execution.
        /// </summary>
        /// <param name="request"> The request to enqueue. </param>
        /// <returns></returns>
        ValueTask EnqueueRequestAsync(IRestRequest request);
    }
}