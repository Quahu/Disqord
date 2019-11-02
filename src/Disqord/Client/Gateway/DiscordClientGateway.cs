using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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

        internal (int ShardId, int ShardCount)? _shards;
        internal DiscordClientState State => _client.State;
        internal IJsonSerializer Serializer => _client.Serializer;
        internal ILogger Logger => _client.Logger;

        private readonly DiscordClientBase _client;
        private readonly IWebSocketClient _ws;

        private CancellationTokenSource _connectionCts;
        private DateTimeOffset? _lastGuildCreate;
        private bool _disposed;
        private bool _manualDisconnection;
        private readonly Random _random = new Random();
        private int? _lastSequenceNumber;
        private string _sessionId;
        private string[] _trace;
        private volatile bool _resuming;
        private volatile bool _reconnecting;
        private readonly object _reconnectionLock = new object();
        private readonly SemaphoreSlim _resumeSemaphore = new SemaphoreSlim(1, 1);
        private TaskCompletionSource<bool> _readyTaskCompletionSource;
        private readonly ConcurrentQueue<(PayloadModel, GatewayDispatch)> _readyPayloadQueue = new ConcurrentQueue<(PayloadModel, GatewayDispatch)>();

        public DiscordClientGateway(DiscordClientBase client, (int ShardId, int ShardCount)? shards, IWebSocketClient websocket)
        {
            _client = client;
            _shards = shards;
            _ws = websocket;
            _ws.MessageReceived += WebSocketMessageReceivedAsync;
            _ws.Closed += WebSocketClosedAsync;
        }

        internal void Log(LogMessageSeverity severity, string message, Exception exception = null)
            => Logger.Log(this, new MessageLoggedEventArgs("Gateway", severity, message, exception));

        public async Task ConnectAsync(string gatewayUrl)
        {
            try
            {
                await _resumeSemaphore.WaitAsync().ConfigureAwait(false);
                var connectionCts = _connectionCts ?? (_connectionCts = new CancellationTokenSource());
                using (var cts = new CancellationTokenSource(10000))
                using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cts.Token, connectionCts.Token))
                {
                    await _ws.ConnectAsync(new Uri(gatewayUrl + RestRequest.BuildQueryString(new Dictionary<string, object>
                    {
                        ["compress"] = "zlib-stream"
                    })), linkedCts.Token).ConfigureAwait(false);
                }

                if (_sessionId != null)
                {
                    _resuming = true;
                    await SendResumeAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                _resumeSemaphore.Release();
            }
        }

        public async Task DisconnectAsync()
        {
            lock (_reconnectionLock)
            {
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
                    _connectionCts?.Cancel();
                }
                catch { }
                _connectionCts?.Dispose();
            }
            await _ws.CloseAsync().ConfigureAwait(false);
        }

        private async Task WebSocketMessageReceivedAsync(WebSocketMessageReceivedEventArgs e)
        {
            var payload = Serializer.Deserialize<PayloadModel>(e.Stream);
            if (_client.IsBot)
            {
                Log(LogMessageSeverity.Debug, $"Received opcode {payload.Op}.");
            }
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
            bool manualDisconnection;
            bool reconnecting;

            lock (_reconnectionLock)
            {
                manualDisconnection = _manualDisconnection;
                reconnecting = _reconnecting;
            }

            if (manualDisconnection)
                return;

            Log(LogMessageSeverity.Warning, $"Close: {e.Status} {e.Description}", e.Exception);
            if (reconnecting)
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
                Log(LogMessageSeverity.Error, $"Close {gatewayCloseCode} ({(int) gatewayCloseCode}) is unrecoverable, not retrying.");
                _reconnecting = false;
                await _client.DisconnectAsync().ConfigureAwait(false);
                return;
            }

            try
            {
                _heartbeatCts?.Cancel();
            }
            catch { }
            _heartbeatCts?.Dispose();
            _reconnecting = true;
            while (!_disposed)
            {
                try
                {
                    await _client.ConnectAsync().ConfigureAwait(false);
                    _reconnecting = false;
                    return;
                }
                catch (SessionLimitException ex)
                {
                    Log(LogMessageSeverity.Critical, $"No available sessions. Resets after {ex.ResetsAfter}.", ex);
                    return;
                }
                catch (DiscordHttpException ex) when ((int) ex.HttpStatusCode == 403)
                {
                    Log(LogMessageSeverity.Critical, "The token is invalid.");
                    return;
                }
                catch (Exception ex)
                {
                    Log(LogMessageSeverity.Error, $"Failed to reconnect after closure. Retrying in 10 seconds.", ex);
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

        }
    }
}
