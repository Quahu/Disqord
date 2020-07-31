using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientGateway
    {
        private readonly Random _random = new Random();

        private async Task HandleOpcodeAsync(PayloadModel payload)
        {
            switch (payload.Op)
            {
                case GatewayOperationCode.Dispatch:
                {
                    try
                    {
                        await HandleDispatchAsync(payload).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Log(LogSeverity.Error, $"An exception occurred while handling a {payload.T} dispatch.\n{payload.D}", ex);
                    }
                    break;
                }

                case GatewayOperationCode.Heartbeat:
                {
                    Log(LogSeverity.Debug, "Heartbeat requested. Sending...");
                    await SendHeartbeatAsync(CancellationToken.None).ConfigureAwait(false);
                    break;
                }

                case GatewayOperationCode.Reconnect:
                {
                    Log(LogSeverity.Information, "Reconnect requested, closing...");
                    _heartbeatCts?.Cancel();
                    await _ws.CloseAsync().ConfigureAwait(false);
                    break;
                }

                case GatewayOperationCode.InvalidSession:
                {
                    Log(LogSeverity.Warning, "Received invalid session...");
                    if (_sessionId != null)
                    {
                        _sessionId = null;
                        var delay = _random.Next(1000, 5001);
                        Log(LogSeverity.Information, $"Currently resuming, starting a new session in {delay}ms.");
                        await Task.Delay(delay).ConfigureAwait(false);
                        await SendIdentifyAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        if (payload.D.ToType<bool>())
                        {
                            Log(LogSeverity.Information, "Session is resumable, resuming...");
                            await SendResumeAsync().ConfigureAwait(false);
                        }
                        else
                        {
                            _sessionId = null;
                            Log(LogSeverity.Information, "Session is not resumable, identifying...");
                            await SendIdentifyAsync().ConfigureAwait(false);
                        }
                    }
                    break;
                }

                case GatewayOperationCode.Hello:
                {
                    var data = payload.D.ToType<HelloModel>();
                    _heartbeatInterval = data.HeartbeatInterval;
                    _ = Task.Run(RunHeartbeatAsync);

                    if (_sessionId != null)
                    {
                        Log(LogSeverity.Information, "Session id is present, attempting to resume...");
                        await SendResumeAsync().ConfigureAwait(false);
                        return;
                    }

                    Log(LogSeverity.Information, "Identifying...");
                    await SendIdentifyAsync().ConfigureAwait(false);
                    break;
                }

                case GatewayOperationCode.HeartbeatAck:
                {
                    Log(LogSeverity.Debug, "Acknowledged Heartbeat.");
                    _lastHeartbeatAck = DateTimeOffset.UtcNow;
                    _lastHeartbeatSent = _lastHeartbeatSend;
                    break;
                }
            }
        }
    }
}
