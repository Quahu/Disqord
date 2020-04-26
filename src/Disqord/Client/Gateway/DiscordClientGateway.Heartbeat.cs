using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Disqord.Logging;

namespace Disqord
{
    internal sealed partial class DiscordClientGateway
    {
        private DateTimeOffset? _lastHeartbeatAck;
        private DateTimeOffset? _lastHeartbeatSent;
        private DateTimeOffset? _lastHeartbeatSend;

        private EfficientCancellationTokenSource _heartbeatCts;
        private int _heartbeatInterval;

        private async Task RunHeartbeatAsync()
        {
            _heartbeatCts?.Cancel();
            _heartbeatCts?.Dispose();
            using (_heartbeatCts = new EfficientCancellationTokenSource())
            {
                var token = _heartbeatCts.Token;
                while (!token.IsCancellationRequested)
                {
                    Log(LogMessageSeverity.Debug, $"Heartbeat: delaying for {_heartbeatInterval}ms.");
                    try
                    {
                        await Task.Delay(_heartbeatInterval, token).ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        Log(LogMessageSeverity.Debug, "Heartbeat: delay cancelled.");
                        return;
                    }

                    Log(LogMessageSeverity.Debug, "Heartbeat: Sending...");
                    var success = false;
                    while (!success && !token.IsCancellationRequested)
                    {
                        try
                        {
                            await SendHeartbeatAsync(token).ConfigureAwait(false);
                            _lastHeartbeatSend = DateTimeOffset.UtcNow;
                            success = true;
                        }
                        catch (WebSocketException ex) when (ex.WebSocketErrorCode == WebSocketError.InvalidState)
                        {
                            Log(LogMessageSeverity.Debug, "Heartbeat: send errored - websocket is in an invalid state.");
                            return;
                        }
                        catch (WebSocketException ex)
                        {
                            Log(LogMessageSeverity.Debug, $"Heartbeat: send errored - {ex.WebSocketErrorCode}.");
                            return;
                        }
                        catch (TaskCanceledException)
                        {
                            Log(LogMessageSeverity.Debug, "Heartbeat: send cancelled.");
                            return;
                        }
                        catch (Exception ex)
                        {
                            Log(LogMessageSeverity.Debug, $"Heartbeat: send failed. Retrying in 5 seconds.", ex);
                            await Task.Delay(5000).ConfigureAwait(false);
                        }
                    }
                }
            }
        }
    }
}
