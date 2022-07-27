using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Qommon.Binding;

namespace Disqord.Gateway.Api;

public interface IGatewayHeartbeater : IBindable<IShard>, ILogging
{
    /// <summary>
    ///     Gets the shard of this heartbeater.
    /// </summary>
    IShard Shard { get; }

    TimeSpan Interval { get; }

    TimeSpan? Latency { get; }

    ValueTask StartAsync(TimeSpan interval, CancellationToken stoppingToken);

    ValueTask StopAsync();

    Task HeartbeatAsync(CancellationToken cancellationToken);

    ValueTask AcknowledgeAsync();
}
