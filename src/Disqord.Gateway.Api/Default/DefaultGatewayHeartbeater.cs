using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Disqord.Utilities.Binding;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Api.Default
{
    public class DefaultGatewayHeartbeater : IGatewayHeartbeater
    {
        public ILogger Logger => ApiClient.Logger;

        public IGatewayApiClient ApiClient => _binder.Value;

        public TimeSpan Interval { get; private set; }

        public TimeSpan? Latency => _lastAcknowledge - _lastSend;

        private DateTime? _lastSend;
        private DateTime? _lastAcknowledge;

        private Cts _cts;
        private Task _task;
        private readonly Binder<IGatewayApiClient> _binder;

        public DefaultGatewayHeartbeater(
            IOptions<DefaultGatewayHeartbeaterConfiguration> options)
        {
            _binder = new Binder<IGatewayApiClient>(this);
        }

        public void Bind(IGatewayApiClient apiClient)
        {
            _binder.Bind(apiClient);
        }

        public ValueTask StartAsync(TimeSpan interval)
        {
            Interval = interval;
            _lastSend = null;
            _lastAcknowledge = null;
            _cts = new Cts();
            _task = Task.Run(InternalRunAsync, _cts.Token);
            return default;
        }

        private async Task InternalRunAsync()
        {
            try
            {
                var cancellationToken = _cts.Token;
                while (!cancellationToken.IsCancellationRequested)
                {
                    Logger.LogTrace("Delaying heartbeat for {0}ms.", Interval.TotalMilliseconds);
                    await Task.Delay(Interval, cancellationToken).ConfigureAwait(false);
                    Logger.LogDebug("Heartbeating...");
                    await HeartbeatAsync(cancellationToken).ConfigureAwait(false);
                }
            }
            catch (TaskCanceledException)
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
            => new()
            {
                Op = GatewayPayloadOperation.Heartbeat,
                D = ApiClient.Serializer.GetJsonNode(ApiClient.Sequence)
            };

        public Task HeartbeatAsync(CancellationToken cancellationToken = default)
        {
            _lastSend = DateTime.Now;
            var payload = GetPayload();
            if (payload.S == null)
                Logger.LogDebug("Heartbeating with a null sequence!");

            return ApiClient.SendAsync(payload, cancellationToken);
        }

        public ValueTask AcknowledgeAsync()
        {
            _lastAcknowledge = DateTime.Now;
            if (Latency != null)
                Logger.LogDebug("Heartbeat acknowledged. Latency: {0}ms.", (int) Latency.Value.TotalMilliseconds);

            return default;
        }
    }
}
