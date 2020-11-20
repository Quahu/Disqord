using System;
using System.Collections.Generic;
using System.IO;
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
using Disqord.Sharding;
using Disqord.WebSocket;

namespace Disqord
{
    internal sealed partial class DiscordClientGateway : IAsyncDisposable
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
        private volatile bool _reconnecting;
        private TaskCompletionSource<bool> _readyTaskCompletionSource;
        private EfficientCancellationTokenSource _combinedRunCts;
        private EfficientCancellationTokenSource _runCts;
        private TaskCompletionSource<bool> _runTcs;
        private TaskCompletionSource<bool> _identifyTcs;
        private readonly WebSocketClient _ws;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        private readonly Queue<(PayloadModel, GatewayDispatch)> _readyPayloadQueue = new Queue<(PayloadModel, GatewayDispatch)>();

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

        internal void Log(LogSeverity severity, string message, Exception exception = null)
            => State.Logger.Log(Client, new LogEventArgs(_shard != null
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
                _runCts = new EfficientCancellationTokenSource();
                _runTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
                _combinedRunCts = EfficientCancellationTokenSource.CreateLinkedTokenSource(_runCts.Token, cancellationToken);
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

            Log(LogSeverity.Information, "Connecting...");
            var gatewayUrl = await Client.GetGatewayAsync(_sessionId == null).ConfigureAwait(false);
            var uri = new Uri(string.Concat(gatewayUrl, "?compress=zlib-stream"));
            Log(LogSeverity.Information, $"Gateway: {uri}");
            await _ws.ConnectAsync(uri, _combinedRunCts.Token).ConfigureAwait(false);
        }

        public Task WaitForIdentifyAsync()
            => _identifyTcs.Task;

        private Task DisconnectAsync()
        {
            Log(LogSeverity.Information, "Disconnecting...");
            return _ws.CloseAsync();
        }

        private void CancelRun(Optional<Exception> exception = default)
        {
            _manualDisconnection = true;
            _sessionId = null;
            _heartbeatCts?.Cancel();
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

                Log(LogSeverity.Information, "Stopping...");
                CancelRun();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task WebSocketMessageReceivedAsync(WebSocketMessageReceivedEventArgs e)
        {
            PayloadModel payload;
            try
            {
                var stream = new MemoryStream();
                e.Stream.CopyTo(stream);
                stream.TryGetBuffer(out var buffer);
                payload = Serializer.Deserialize<PayloadModel>(buffer);
            }
            catch (Exception ex)
            {
                Log(LogSeverity.Critical, $"An exception occurred while trying to deserialize a payload.", ex);
                return;
            }

            Log(LogSeverity.Debug, $"Received opcode {payload.Op}.");
            try
            {
                await HandleOpcodeAsync(payload).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log(LogSeverity.Error, $"An exception occurred while handling opcode {payload.Op} ({(byte) payload.Op}).", ex);
            }
        }

        private async Task WebSocketClosedAsync(WebSocketClosedEventArgs e)
        {
            try
            {
                if (_manualDisconnection)
                    return;

                if (_reconnecting)
                    return;

                await _semaphore.WaitAsync().ConfigureAwait(false);
                _reconnecting = true;

                Log(LogSeverity.Warning, $"Close: {e.Status} {e.Description}", e.Exception);
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
                        Log(LogSeverity.Critical, message);
                        _reconnecting = false;
                        var exception = new Exception(message);
                        _identifyTcs.TrySetException(exception);
                        CancelRun(exception);
                        return;
                    }
                }

                _heartbeatCts?.Cancel();
                while (!_combinedRunCts.IsCancellationRequested && !_isDisposed)
                {
                    try
                    {
                        Log(LogSeverity.Information, "Attempting to reconnect after the close...");
                        await ConnectAsync().ConfigureAwait(false);
                        _reconnecting = false;
                        return;
                    }
                    catch (SessionLimitException ex)
                    {
                        Log(LogSeverity.Critical, $"No available sessions. Resets after {ex.ResetsAfter}.", ex);
                        CancelRun(ex);
                        return;
                    }
                    catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.Unauthorized)
                    {
                        Log(LogSeverity.Critical, "Invalid token.");
                        CancelRun(ex);
                        return;
                    }
                    catch (Exception ex)
                    {
                        Log(LogSeverity.Error, $"Failed to reconnect. Retrying in 10 seconds.", ex);
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
                DateTimeOffset? lastGuildCreate;
                var now = DateTimeOffset.UtcNow;

                // Delay up to ~3 seconds for the first GUILD_CREATE.
                while ((lastGuildCreate = _lastGuildCreate) == null && (DateTimeOffset.UtcNow - now).TotalSeconds < 3)
                    await Task.Delay(100).ConfigureAwait(false);

                // If we received a guild continue to receive them
                // with a ~3.5 seconds window.
                if (lastGuildCreate != null)
                {
                    while ((DateTimeOffset.UtcNow - lastGuildCreate.Value).TotalSeconds < 3.5)
                    {
                        await Task.Delay(100).ConfigureAwait(false);
                        lastGuildCreate = _lastGuildCreate;

                    }

                    var guilds = State._guilds.ToArray().Where(x => x.Value.Client.GetGateway(x.Key) == this && !x.Value.IsChunked)
                        .Select(x => x.Value)
                        .ToArray();
                    var tasks = new Task[guilds.Length];
                    for (var i = 0; i < guilds.Length; i++)
                    {
                        var guild = guilds[i];
                        Log(LogSeverity.Information, $"Requesting members for {guild.Name}. Expecting {guild.ChunksExpected} {(guild.ChunksExpected == 1 ? "chunk" : "chunks")}.");
                        await SendRequestMembersAsync(guild.Id).ConfigureAwait(false);
                        tasks[i] = guild.ChunkTcs.Task;

                        if (i + 1 % 115 == 0)
                        {
                            Log(LogSeverity.Information, "Delaying chunk requests for 1 minute.");
                            await Task.Delay(60_000).ConfigureAwait(false);
                        }
                    }

                    var timeoutTask = Task.Delay(40_000 + guilds.Length / 115 * 60_000);
                    var batchesTask = Task.WhenAll(tasks);
                    var task = await Task.WhenAny(timeoutTask, batchesTask).ConfigureAwait(false);
                    if (task == timeoutTask)
                    {
                        Log(LogSeverity.Critical, $"Timed out waiting for member chunks.");
                    }
                }
            }
            else
            {
                var guilds = State._guilds.Values;
                Log(LogSeverity.Information, $"Awaiting sync for {guilds.Count} guilds.");
                await Task.WhenAll(guilds.Select(x => x.SyncTcs.Task)).ConfigureAwait(false);
            }

            if (_shard == null)
            {
                await Client._ready.InvokeAsync(new ReadyEventArgs(Client, _sessionId, _trace)).ConfigureAwait(false);
            }
            else if (Client is IDiscordSharder sharder)
            {
                var shardId = _shard[0];
                var args = new ShardReadyEventArgs(Client, _sessionId, _trace, sharder.Shards[shardId]);
                await sharder._shardReady.InvokeAsync(args).ConfigureAwait(false);

                if (shardId == sharder.Shards.Count - 1)
                    await Client._ready.InvokeAsync(args).ConfigureAwait(false);
            }

            while (true)
            {
                (PayloadModel, GatewayDispatch) queuedPayload;
                lock (_readyPayloadQueue)
                {
                    var dequeued = _readyPayloadQueue.TryDequeue(out queuedPayload);
                    if (!dequeued)
                    {
                        _readyTaskCompletionSource.SetResult(true);
                        _readyTaskCompletionSource = null;
                        break;
                    }
                }

                Log(LogSeverity.Debug, $"Firing queued up payload: {queuedPayload.Item2} with S: {queuedPayload.Item1.S}.");
                try
                {
                    await HandleDispatchAsync(queuedPayload.Item1, queuedPayload.Item2).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Log(LogSeverity.Error, $"An exception occurred while handling a queued {queuedPayload.Item1.T} dispatch.\n{queuedPayload.Item1.D}", ex);
                }
            }
        }

        public ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return default;

            _isDisposed = true;
            CancelRun(null);
            return _ws.DisposeAsync();
        }
    }
}
