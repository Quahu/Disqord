using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;

namespace Disqord
{
    internal sealed partial class DiscordClientGateway : IDisposable
    {
        private DateTimeOffset? _lastHeartbeatAck;
        private DateTimeOffset? _lastHeartbeatSent;
        private DateTimeOffset? _lastHeartbeatSend;

        private CancellationTokenSource _heartbeatCts;
        private int _heartbeatInterval;

        private async Task RunHeartbeatAsync()
        {
            try
            {
                _heartbeatCts?.Cancel();
            }
            catch { }
            _heartbeatCts?.Dispose();
            _heartbeatCts = new CancellationTokenSource();
            while (!_heartbeatCts.IsCancellationRequested)
            {
                Log(LogMessageSeverity.Debug, $"Heartbeat: delaying for {_heartbeatInterval}ms.");
                try
                {
                    await Task.Delay(_heartbeatInterval, _heartbeatCts.Token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    Log(LogMessageSeverity.Warning, "Heartbeat: delay cancelled, returning.");
                    return;
                }

                Log(LogMessageSeverity.Debug, "Heartbeat: Sending...");
                var success = false;
                while (!success && !_heartbeatCts.IsCancellationRequested)
                {
                    try
                    {
                        await SendHeartbeatAsync().ConfigureAwait(false);
                        success = true;
                    }
                    catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.InvalidState)
                    {
                        Log(LogMessageSeverity.Error, "Heartbeat: send errored - websocket in an invalid state. Returning.");
                        return;
                    }
                    catch (WebSocketException ex)
                    {
                        Log(LogMessageSeverity.Error, $"Heartbeat: send errored - {ex.WebSocketErrorCode}. Returning.");
                        return;
                    }
                    catch (TaskCanceledException)
                    {
                        Log(LogMessageSeverity.Error, "Heartbeat: send cancelled. Returning.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log(LogMessageSeverity.Error, $"Heartbeat: send failed. Retrying in 5 seconds.", ex);
                        await Task.Delay(5000).ConfigureAwait(false);
                    }
                }
                _lastHeartbeatSend = DateTimeOffset.UtcNow;
            }
        }
    }
}
