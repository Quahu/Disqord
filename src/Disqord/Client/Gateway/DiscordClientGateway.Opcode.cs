using System;
using System.Threading.Tasks;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Models.Dispatches;

namespace Disqord
{
    internal sealed partial class DiscordClientGateway : IDisposable
    {
        private async Task HandleOpcodeAsync(PayloadModel payload)
        {
            switch (payload.Op)
            {
                case Opcode.Dispatch:
                {
                    await HandleDispatchAsync(payload).ConfigureAwait(false);
                    break;
                }

                case Opcode.Heartbeat:
                {
                    Log(LogMessageSeverity.Debug, "Heartbeat requested. Heartbeating...");
                    await SendHeartbeatAsync().ConfigureAwait(false);
                    break;
                }

                case Opcode.Reconnect:
                {
                    Log(LogMessageSeverity.Information, "Reconnect requested, closing...");
                    try
                    {
                        _heartbeatCts?.Cancel();
                    }
                    catch { }
                    _heartbeatCts?.Dispose();
                    await _ws.CloseAsync().ConfigureAwait(false);
                    break;
                }

                case Opcode.InvalidSession:
                {
                    Log(LogMessageSeverity.Warning, "Received invalid session...");
                    if (_resuming)
                    {
                        Log(LogMessageSeverity.Information, "Currently resuming, starting a new session...");
                        await Task.Delay(_random.Next(1000, 5001)).ConfigureAwait(false);
                        await SendIdentifyAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        if ((bool) payload.D)
                        {
                            Log(LogMessageSeverity.Information, "Session is resumable, resuming...");
                            await SendResumeAsync().ConfigureAwait(false);
                            _resuming = true;
                        }
                        else
                        {
                            Log(LogMessageSeverity.Information, "Session is not resumable, identifying...");
                            await SendIdentifyAsync().ConfigureAwait(false);
                        }
                    }
                    break;
                }

                case Opcode.Hello:
                {
                    var data = Serializer.ToObject<HelloModel>(payload.D);
                    _heartbeatInterval = data.HeartbeatInterval;
                    _ = RunHeartbeatAsync();
                    try
                    {
                        await _resumeSemaphore.WaitAsync().ConfigureAwait(false);
                        if (_resuming)
                        {
                            Log(LogMessageSeverity.Information, "Received Hello after requesting a resume, not identifying.");
                            return;
                        }
                    }
                    finally
                    {
                        _resumeSemaphore.Release();
                    }

                    Log(LogMessageSeverity.Information, "Received Hello, identifying...");
                    await SendIdentifyAsync().ConfigureAwait(false);
                    break;
                }

                case Opcode.HeartbeatAck:
                {
                    _lastHeartbeatSent = _lastHeartbeatSend;
                    _lastHeartbeatAck = DateTimeOffset.UtcNow;
                    Log(LogMessageSeverity.Debug, "Acknowledged Heartbeat.");
                    break;
                }
            }
        }
    }
}
