using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Logging;
using Disqord.Utilities.Binding;

namespace Disqord.Gateway.Api
{
    public interface IGatewayRateLimiter : IBindable<IGatewayApiClient>, ILogging, IDisposable
    {
        IGatewayApiClient ApiClient { get; }

        /// <summary>
        ///     Gets whether the given operation is currently rate-limited.
        ///     If given <see langword="null"/>, checks only the master bucket. Otherwise it checks both the operation bucket, if one exists, and the master bucket.
        /// </summary>
        /// <param name="operation"> The operation to check. </param>
        /// <returns>
        ///     <see langword="true"/> if rate-limited.
        /// </returns>
        bool IsRateLimited(GatewayPayloadOperation? operation = null);

        Task WaitAsync(GatewayPayloadOperation? operation = null, CancellationToken cancellationToken = default);

        void Release(GatewayPayloadOperation? operation = null);
    }
}
