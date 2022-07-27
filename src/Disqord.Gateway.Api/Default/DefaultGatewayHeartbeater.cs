using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Binding;

namespace Disqord.Gateway.Api.Default;

public class DefaultGatewayHeartbeater : IGatewayHeartbeater
{
    public ILogger Logger => Shard.Logger;

    public IShard Shard => _binder.Value;

    public TimeSpan Interval { get; private set; }

    public TimeSpan? Latency => _lastAcknowledge - _lastSend;

    private DateTime? _lastSend;
    private DateTime? _lastAcknowledge;

    private Cts? _cts;
    private Task? _task;
    private readonly Binder<IShard> _binder;

    public DefaultGatewayHeartbeater(
        IOptions<DefaultGatewayHeartbeaterConfiguration> options)
    {
        _binder = new Binder<IShard>(this);
    }

    public void Bind(IShard apiClient)
    {
        _binder.Bind(apiClient);
    }

    public ValueTask StartAsync(TimeSpan interval, CancellationToken stoppingToken)
    {
        Interval = interval;
        _lastSend = null;
        _lastAcknowledge = null;
        _cts = Cts.Linked(stoppingToken);
        _task = Task.Run(InternalRunAsync, _cts.Token);
        return default;
    }

    private async Task InternalRunAsync()
    {
        try
        {
            var cancellationToken = _cts!.Token;
            while (!cancellationToken.IsCancellationRequested)
            {
                Logger.LogTrace("Delaying heartbeat for {0}ms.", Interval.TotalMilliseconds);
                await Task.Delay(Interval, cancellationToken).ConfigureAwait(false);
                Logger.LogDebug("Heartbeating...");
                await HeartbeatAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while heartbeating.");
        }
    }

    public ValueTask StopAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        return default;
    }

    protected virtual GatewayPayloadJsonModel GetPayload()
    {
        return new GatewayPayloadJsonModel
        {
            Op = GatewayPayloadOperation.Heartbeat,
            D = Shard.Serializer.GetJsonNode(Shard.Sequence)
        };
    }

    public Task HeartbeatAsync(CancellationToken cancellationToken = default)
    {
        _lastSend = DateTime.Now;
        return Shard.SendAsync(GetPayload(), cancellationToken);
    }

    public ValueTask AcknowledgeAsync()
    {
        _lastAcknowledge = DateTime.Now;
        if (Latency != null)
            Logger.LogDebug("Heartbeat acknowledged. Latency: {0}ms.", (int) Latency.Value.TotalMilliseconds);

        return default;
    }
}
