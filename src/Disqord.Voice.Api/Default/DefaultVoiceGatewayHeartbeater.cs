using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Disqord.Voice.Api.Models;
using Microsoft.Extensions.Logging;
using Qommon.Binding;

namespace Disqord.Voice.Api.Default;

public class DefaultVoiceGatewayHeartbeater : IVoiceGatewayHeartbeater
{
    public ILogger Logger => Client.Logger;

    public IVoiceGatewayClient Client => _binder.Value;

    public TimeSpan Interval { get; private set; }

    public TimeSpan? Latency => _lastAcknowledge - _lastSend;

    private DateTime? _lastSend;
    private DateTime? _lastAcknowledge;
    private uint _sequence;

    private Cts? _cts;
    private Task? _task;
    private readonly Binder<IVoiceGatewayClient> _binder;

    public DefaultVoiceGatewayHeartbeater()
    {
        _binder = new Binder<IVoiceGatewayClient>(this);
    }

    public void Bind(IVoiceGatewayClient value)
    {
        _binder.Bind(value);
    }

    public ValueTask StartAsync(TimeSpan interval)
    {
        Interval = interval;
        _lastSend = null;
        _lastAcknowledge = null;
        _sequence = 0;
        _cts = new Cts();
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
                Logger.LogTrace("Delaying voice heartbeat for {0}ms.", Interval.TotalMilliseconds);
                await Task.Delay(Interval, cancellationToken).ConfigureAwait(false);
                Logger.LogDebug("Voice heartbeating...");
                await HeartbeatAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            Logger.LogError(ex, "An exception occurred while voice heartbeating.");
        }
    }

    public ValueTask StopAsync()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        return default;
    }

    protected virtual VoiceGatewayPayloadJsonModel GetPayload()
    {
        return new()
        {
            Op = VoiceGatewayPayloadOperation.Heartbeat,
            D = Client.Serializer.GetJsonNode(_sequence++)
        };
    }

    public Task HeartbeatAsync(CancellationToken cancellationToken = default)
    {
        _lastSend = DateTime.Now;
        return Client.SendAsync(GetPayload(), cancellationToken);
    }

    public ValueTask AcknowledgeAsync()
    {
        _lastAcknowledge = DateTime.Now;
        if (Latency != null)
            Logger.LogDebug("Voice heartbeat acknowledged. Latency: {0}ms.", (int) Latency.Value.TotalMilliseconds);

        return default;
    }

    public virtual void Dispose()
    { }
}
