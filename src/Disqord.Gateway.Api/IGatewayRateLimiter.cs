using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Qommon.Binding;

namespace Disqord.Gateway.Api;

public interface IGatewayRateLimiter : IBindable<IShard>, ILogging
{
    /// <summary>
    ///     Gets the shard of this rate-limiter.
    /// </summary>
    IShard Shard { get; }

    /// <summary>
    ///     Gets whether the given operation is currently rate-limited.
    ///     If given <see langword="null"/>, checks only the master bucket. Otherwise it checks both the operation bucket, if one exists, and the master bucket.
    /// </summary>
    /// <param name="operation"> The operation to check. </param>
    /// <returns>
    ///     <see langword="true"/> if rate-limited.
    /// </returns>
    bool IsRateLimited(GatewayPayloadOperation? operation = null);

    /// <summary>
    ///     Gets the amount of remaining requests for the given operation before it is rate-limited.
    ///     If given <see langword="null"/>, checks the master bucket. Otherwise it checks the operation bucket, if one exists.
    /// </summary>
    /// <param name="operation"> The operation to check. </param>
    /// <returns>
    ///     The amount of remaining requests.
    /// </returns>
    int GetRemainingRequests(GatewayPayloadOperation? operation = null);

    Task WaitAsync(GatewayPayloadOperation? operation = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Notifies the rate-limiter that an operation was successfully completed.
    /// </summary>
    /// <param name="operation"> The operation completed. </param>
    void NotifyCompletion(GatewayPayloadOperation? operation = null);

    void Release(GatewayPayloadOperation? operation = null);

    void Reset();
}
