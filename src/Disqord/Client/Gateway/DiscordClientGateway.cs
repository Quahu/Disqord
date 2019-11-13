using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Events;
using Disqord.Logging;
using Disqord.Models;
using Disqord.Rest;
using Disqord.Serialization.Json;
using Disqord.WebSocket;

namespace Disqord
{
    internal sealed partial class DiscordClientGateway : IDisposable
    {
        public TimeSpan? Latency => _lastHeartbeatAck - _lastHeartbeatSent;

        internal IJsonSerializer Serializer => _client.Serializer;
        internal DiscordClientState State => _client.State;

        private readonly DiscordClientBase _client;

        private bool _isDisposed;
        private DateTimeOffset? _lastGuildCreate;
        private bool _manualDisconnection;
        private int? _lastSequenceNumber;
        private string _sessionId;
        private string[] _trace;
        private volatile bool _resuming;
        private volatile bool _reconnecting;
        private readonly object _reconnectionLock = new object();
        private TaskCompletionSource<bool> _readyTaskCompletionSource;
        private CancellationTokenSource _combinedRunCts;
        private CancellationTokenSource _runCts;
        private TaskCompletionSource<bool> _runTcs;
        private readonly WebSocketClient _ws;

        private readonly ConcurrentQueue<(PayloadModel, GatewayDispatch)> _readyPayloadQueue = new ConcurrentQueue<(PayloadModel, GatewayDispatch)>();

        public DiscordClientGateway(DiscordClientBase client, (int ShardId, int ShardCount)? shards)
        {
            _client = client;
            _shards = shards;
            _ws = new WebSocketClient();
            _ws.MessageReceived += WebSocketMessageReceivedAsync;
            _ws.Closed += WebSocketClosedAsync;
        }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => _client.Logger.Log(this, new MessageLoggedEventArgs("Gateway", severity, message, exception));

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            if (_combinedRunCts != null)
                throw new InvalidOperationException("The gateway is already running.");

            _runCts = new CancellationTokenSource();
            _runTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _combinedRunCts = CancellationTokenSource.CreateLinkedTokenSource(_runCts.Token, cancellationToken);
            _combinedRunCts.Token.Register(() => CancelRun(true));
            await ConnectAsync().ConfigureAwait(false);
            await _runTcs.Task.ConfigureAwait(false);
        }

        private async Task ConnectAsync()
        {
            Log(LogMessageSeverity.Debug, "Connecting...");
            var gatewayUrl = await _client.GetGatewayAsync(_sessionId == null).ConfigureAwait(false);
            await _ws.ConnectAsync(new Uri(string.Concat(gatewayUrl, "?compress=zlib-stream")), _combinedRunCts.Token).ConfigureAwait(false);

            if (_sessionId != null)
            {
                _resuming = true;
                Log(LogMessageSeverity.Debug, "Session id is present, attempting to resume...");
                await SendResumeAsync().ConfigureAwait(false);
            }
        }

        private void CancelRun(bool forced)
        {
            if (_combinedRunCts == null)
                return;

            _manualDisconnection = true;
            _sessionId = null;
            try
            {
                _heartbeatCts?.Cancel();
            }
            catch { }
            _heartbeatCts?.Dispose();
            try
            {
                _runCts?.Cancel();
            }
            catch { }
            try
            {
                _combinedRunCts?.Cancel();
            }
            catch { }
            _combinedRunCts?.Dispose();
            _runCts?.Dispose();
            if (forced)
                _runTcs?.TrySetException(new TaskCanceledException());
            else
                _runTcs.TrySetResult(true);
        }

        public async Task StopAsync()
        {
            if (_combinedRunCts == null)
                throw new InvalidOperationException("The gateway is not running.");

            Log(LogMessageSeverity.Debug, "Stopping...");
            await _ws.CloseAsync().ConfigureAwait(false);
            CancelRun(false);
        }

        private async Task WebSocketMessageReceivedAsync(WebSocketMessageReceivedEventArgs e)
        {
            var payload = Serializer.Deserialize<PayloadModel>(e.Stream);
            Log(LogMessageSeverity.Debug, $"Received opcode {payload.Op}.");
            try
            {
                await HandleOpcodeAsync(payload).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log(LogMessageSeverity.Error, $"An exception occurred while handling opcode {payload.Op} ({(byte) payload.Op}).", ex);
            }
        }

        private async Task WebSocketClosedAsync(WebSocketClosedEventArgs e)
        {
            if (_manualDisconnection)
                return;

            Log(LogMessageSeverity.Warning, $"Close: {e.Status} {e.Description}", e.Exception);
            if (_reconnecting)
                return;

            GatewayCloseCode gatewayCloseCode = default;
            bool shouldRetry;
            if (e.Status != null)
            {
                gatewayCloseCode = (GatewayCloseCode) e.Status.Value;
                switch (gatewayCloseCode)
                {
                    case GatewayCloseCode.AuthenticationFailed:
                    case GatewayCloseCode.RateLimited:
                    case GatewayCloseCode.InvalidShard:
                    case GatewayCloseCode.ShardingRequired:
                        shouldRetry = false;
                        break;

                    default:
                        shouldRetry = true;
                        break;
                }
            }
            else
            {
                shouldRetry = true;
            }

            if (!shouldRetry && e.Status != null)
            {
                Log(LogMessageSeverity.Error, $"Close {gatewayCloseCode} ({(int) gatewayCloseCode}) is unrecoverable, stopping.");
                _reconnecting = false;
                await StopAsync().ConfigureAwait(false);
                return;
            }

            try
            {
                _heartbeatCts?.Cancel();
            }
            catch { }
            _heartbeatCts?.Dispose();
            _sessionId = null;
            _reconnecting = true;
            while (!_combinedRunCts.IsCancellationRequested && !_isDisposed)
            {
                try
                {
                    Log(LogMessageSeverity.Information, "Attempting to reconnect after the close...");
                    await ConnectAsync().ConfigureAwait(false);
                    _reconnecting = false;
                    return;
                }
                catch (SessionLimitException ex)
                {
                    Log(LogMessageSeverity.Critical, $"No available sessions. Resets after {ex.ResetsAfter}.", ex);
                    _runTcs.TrySetException(ex);
                    return;
                }
                catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.Unauthorized)
                {
                    Log(LogMessageSeverity.Critical, "Invalid token.");
                    _runTcs.TrySetException(ex);
                    return;
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    Log(LogMessageSeverity.Error, $"Failed to reconnect. Retrying in 10 seconds.", ex);
                    await Task.Delay(10000).ConfigureAwait(false);
                }
            }
        }

        private async Task DelayedInvokeReadyAsync()
        {
            if (_client.IsBot)
            {
                var last = _lastGuildCreate;
                while (last == null || (DateTimeOffset.UtcNow - last).Value.TotalSeconds < 2)
                {
                    await Task.Delay(last == null ? 2000 : 1000).ConfigureAwait(false);
                    last = _lastGuildCreate;
                }

                var batches = State._guilds.Values.Where(x => !x.IsChunked).Batch(75).Select(x => x.ToArray()).ToArray();
                var tasks = new Task[batches.Length];
                for (var i = 0; i < batches.Length; i++)
                {
                    var batch = batches[i];
                    for (var j = 0; j < batch.Length; j++)
                    {
                        var guild = batch[j];
                        Log(LogMessageSeverity.Information, $"Requesting offline members for {guild.Name}. Expecting {guild.ChunksExpected} {(guild.ChunksExpected == 1 ? "chunk" : "chunks")}.");
                    }

                    await SendRequestOfflineMembersAsync(batch.Select(x => x.Id.RawValue).ToArray()).ConfigureAwait(false);
                    tasks[i] = Task.WhenAll(batch.Select(x => x.ChunkTcs.Task));
                }

                await Task.WhenAll(tasks).ConfigureAwait(false);
            }
            else
            {
                var guilds = State._guilds.Values;
                Log(LogMessageSeverity.Information, $"Awaiting sync for {guilds.Count} guilds.");
                await Task.WhenAll(guilds.Select(x => x.SyncTcs.Task)).ConfigureAwait(false);
            }

            // TODO
            if (_shards == null)
                await _client._ready.InvokeAsync(new ReadyEventArgs(_client, _sessionId, _trace)).ConfigureAwait(false);

            while (_readyPayloadQueue.TryDequeue(out var queuedPayload))
            {
                Log(LogMessageSeverity.Debug, $"Firing queued up payload: {queuedPayload.Item2} with S: {queuedPayload.Item1.S}.");
                await HandleDispatchAsync(queuedPayload.Item1, queuedPayload.Item2).ConfigureAwait(false);
            }

            _readyTaskCompletionSource.SetResult(true);
            _readyTaskCompletionSource = null;
        }

        public void Dispose()
        {
            _isDisposed = true;

            if (!_isDisposed)
            {
                CancelRun(true);
                _ws.Dispose();
            }
        }
    }
}
