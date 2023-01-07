using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Qommon.Binding;

namespace Disqord.Voice.Api;

public interface IVoiceGatewayHeartbeater : IBindable<IVoiceGatewayClient>, ILogging
{
    IVoiceGatewayClient Client { get; }

    TimeSpan Interval { get; }

    TimeSpan? Latency { get; }

    ValueTask StartAsync(TimeSpan interval);

    ValueTask StopAsync();

    Task HeartbeatAsync(CancellationToken cancellationToken = default);

    ValueTask AcknowledgeAsync();
}
