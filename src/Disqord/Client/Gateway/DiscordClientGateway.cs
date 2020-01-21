using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Client.Gateway;
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

        internal IJsonSerializer Serializer => State.Serializer;
        internal DiscordClientBase Client => State._client;
        internal readonly DiscordClientState State;

        private readonly IdentifyLock _identifyLock;

        private bool _isDisposed;
        private DateTimeOffset? _lastGuildCreate;
        private bool _manualDisconnection;
        private int? _lastSequenceNumber;
        private string _sessionId;
        private string[] _trace;
        private volatile bool _resuming;
        private volatile bool _reconnecting;
        private TaskCompletionSource<bool> _readyTaskCompletionSource;
        private CancellationTokenSource _combinedRunCts;
        private CancellationTokenSource _runCts;
        private TaskCompletionSource<bool> _runTcs;
        private TaskCompletionSource<bool> _identifyTcs;
        private readonly WebSocketClient _ws;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private readonly ConcurrentQueue<(PayloadModel, GatewayDispatch)> _readyPayloadQueue = new ConcurrentQueue<(PayloadModel, GatewayDispatch)>();

        public DiscordClientGateway(DiscordClientState state, (int ShardId, int ShardCount)? shards)
        {
            State = state;
            _identifyLock = new IdentifyLock(this);
            _shard = shards != null
                ? new[] { shards.Value.ShardId, shards.Value.ShardCount }
                : null;

            _ws = new WebSocketClient();
            _ws.MessageReceived += WebSocketMessageReceivedAsync;
            _ws.Closed += WebSocketClosedAsync;
        }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => State.Logger.Log(Client, new MessageLoggedEventArgs(_shard != null
                ? $"Gateway #{_shard[0]}"
                : "Gateway", severity, message, exception));

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                if (_combinedRunCts != null)
                    throw new InvalidOperationException("The gateway is already running.");

                _manualDisconnection = false;
                _runCts = new CancellationTokenSource();
                _runTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                _combinedRunCts = CancellationTokenSource.CreateLinkedTokenSource(_runCts.Token, cancellationToken);
                _combinedRunCts.Token.Register(x => (x as DiscordClientGateway).CancelRun(null), this);
                await ConnectAsync().ConfigureAwait(false);
            }
            finally
            {
                _semaphore.Release();
            }

            try
            {
                await _runTcs.Task.ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                await DisconnectAsync().ConfigureAwait(false);
                throw;
            }
        }

        private async Task ConnectAsync()
        {
            if (_sessionId == null)
                _identifyTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            Log(LogMessageSeverity.Information, "Connecting...");
            var gatewayUrl = await Client.GetGatewayAsync(_sessionId == null).ConfigureAwait(false);
            await _ws.ConnectAsync(new Uri(string.Concat(gatewayUrl, "?compress=zlib-stream")), _combinedRunCts.Token).ConfigureAwait(false);

            if (_sessionId != null)
            {
                Log(LogMessageSeverity.Information, "Session id is present, attempting to resume...");
                _resuming = true;
                await SendResumeAsync().ConfigureAwait(false);
            }
        }

        public Task WaitForIdentifyAsync()
            => _identifyTcs.Task;

        private Task DisconnectAsync()
        {
            Log(LogMessageSeverity.Information, "Disconnecting...");
            return _ws.CloseAsync();
        }

        private void CancelRun(Optional<Exception> exception = default)
        {
            _manualDisconnection = true;
            _sessionId = null;
            try
            {
                _heartbeatCts?.Cancel();
            }
            catch { }
            _heartbeatCts?.Dispose();
            _combinedRunCts?.Dispose();
            _combinedRunCts = null;
            _runCts?.Dispose();
            _runCts = null;
            if (exception.HasValue)
            {
                if (exception.Value != null)
                {
                    _runTcs?.TrySetException(exception.Value);
                }
                else
                {
                    _runTcs?.TrySetCanceled();
                }
            }
            else
            {
                _runTcs.TrySetResult(true);
            }
        }

        public async Task StopAsync()
        {
            try
            {
                await _semaphore.WaitAsync(_combinedRunCts.Token).ConfigureAwait(false);

                if (_combinedRunCts == null)
                    throw new InvalidOperationException("The gateway is not running.");

                Log(LogMessageSeverity.Information, "Stopping...");
                CancelRun();
            }
            finally
            {
                _semaphore.Release();
            }
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
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                if (_manualDisconnection)
                    return;

                if (_reconnecting)
                    return;

                _reconnecting = true;

                Log(LogMessageSeverity.Warning, $"Close: {e.Status} {e.Description}", e.Exception);
                bool shouldRetry;
                if (e.Status != null)
                {
                    var gatewayCloseCode = (GatewayCloseCode) e.Status.Value;
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

                    if (!shouldRetry)
                    {
                        var message = $"Close {gatewayCloseCode} ({(int) gatewayCloseCode}) is unrecoverable, stopping.";
                        Log(LogMessageSeverity.Critical, message);
                        _reconnecting = false;
                        var exception = new Exception(message);
                        _identifyTcs.TrySetException(exception);
                        CancelRun(exception);
                        return;
                    }
                }

                try
                {
                    _heartbeatCts?.Cancel();
                }
                catch { }
                _heartbeatCts?.Dispose();

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
                        CancelRun(ex);
                        return;
                    }
                    catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.Unauthorized)
                    {
                        Log(LogMessageSeverity.Critical, "Invalid token.");
                        CancelRun(ex);
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
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task DelayedInvokeReadyAsync()
        {
            // TODO: break out on connection interruption
            if (Client.IsBot)
            {
                var last = _lastGuildCreate;
                while (last == null || (DateTimeOffset.UtcNow - last).Value.TotalSeconds < 3)
                {
                    await Task.Delay(last == null ? 3000 : 1500).ConfigureAwait(false);
                    last = _lastGuildCreate;
                }

                var batches = State._guilds.Values.Where(x => x.Client.GetGateway(x.Id) == this && !x.IsChunked).Batch(75).Select(x => x.ToArray()).ToArray();
                var tasks = new Task[batches.Length];
                for (var i = 0; i < batches.Length; i++)
                {
                    var batch = batches[i];
                    for (var j = 0; j < batch.Length; j++)
                    {
                        var guild = batch[j];
                        Log(LogMessageSeverity.Information, $"Requesting offline members for {guild.Name}. Expecting {guild.ChunksExpected} {(guild.ChunksExpected == 1 ? "chunk" : "chunks")}.");
                    }

                    await SendRequestOfflineMembersAsync(batch.Select(x => x.Id.RawValue)).ConfigureAwait(false);
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
            if (_shard == null)
                await Client._ready.InvokeAsync(new ReadyEventArgs(Client, _sessionId, _trace)).ConfigureAwait(false);

            while (_readyPayloadQueue.TryDequeue(out var queuedPayload))
            {
                Log(LogMessageSeverity.Debug, $"Firing queued up payload: {queuedPayload.Item2} with S: {queuedPayload.Item1.S}.");
                try
                {
                    await HandleDispatchAsync(queuedPayload.Item1, queuedPayload.Item2).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log(LogMessageSeverity.Error, $"An exception occurred while handling a queued {queuedPayload.Item1.T} dispatch.\n{queuedPayload.Item1.D}", ex);
                }
            }

            _readyTaskCompletionSource.SetResult(true);
            _readyTaskCompletionSource = null;
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            _isDisposed = true;
            CancelRun(null);
            _ws.Dispose();
        }
    }
}
