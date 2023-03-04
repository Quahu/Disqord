using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Qommon;
using Qommon.Collections.ThreadSafe;

namespace Disqord.Gateway.Default.Dispatcher;

public class ReadyDispatchHandler : DispatchHandler<ReadyJsonModel, ReadyEventArgs>
{
    public IThreadSafeDictionary<ShardId, IThreadSafeDictionary<Snowflake, bool>> PendingGuilds { get; }

    private readonly IThreadSafeDictionary<ShardId, DelayToken> _delays;

    private readonly Tcs _readyTcs;
    private int? _pendingReadyShards;
    private int _readyShards;

    public ReadyDispatchHandler()
    {
        PendingGuilds = ThreadSafeDictionary.Monitor.Create<ShardId, IThreadSafeDictionary<Snowflake, bool>>();

        _delays = ThreadSafeDictionary.Monitor.Create<ShardId, DelayToken>();
        _readyTcs = new();
    }

    private void NotifyShardReady()
    {
        var pendingReadyShards = _pendingReadyShards;
        if (pendingReadyShards == null)
            return;

        if (Interlocked.Increment(ref _readyShards) == pendingReadyShards)
        {
            _readyTcs.Complete();
            _pendingReadyShards = null;
        }
    }

    public override async ValueTask<ReadyEventArgs?> HandleDispatchAsync(IShard shard, ReadyJsonModel model)
    {
        _pendingReadyShards ??= shard.ApiClient.Shards.Count;

        CacheProvider.Reset(shard.Id);
        var pendingGuilds = ThreadSafeDictionary.Monitor.Wrap(model.Guilds.ToDictionary(x => x.Id, x => !x.Unavailable.GetValueOrDefault()));
        PendingGuilds[shard.Id] = pendingGuilds;

        if (Dispatcher.CurrentUser == null)
        {
            // The shared user for the bot is always going to be referenced.
            var sharedUser = new CachedSharedUser(Client, model.User);
            if (CacheProvider.TryGetUsers(out var sharedUserCache))
            {
                sharedUserCache.Add(sharedUser.Id, sharedUser);
            }

            Dispatcher.CurrentUser = new CachedCurrentUser(sharedUser, model.User);
        }
        else
        {
            Dispatcher.CurrentUser.Update(model.User);
        }

        Dispatcher.CurrentApplicationId = model.Application.Id;
        Dispatcher.CurrentApplicationFlags = model.Application.Flags;

        var guildIds = pendingGuilds.Keys;
        var e = new ReadyEventArgs(shard.Id, guildIds, Dispatcher.CurrentUser, Dispatcher.CurrentApplicationId.Value, Dispatcher.CurrentApplicationFlags.Value);
        var delayMode = Dispatcher.ReadyEventDelayMode;
        if (delayMode == ReadyEventDelayMode.None || guildIds.Length == 0)
        {
            shard.Logger.LogInformation("Ready as {0} with {1} pending guilds.", Dispatcher.CurrentUser.Tag, guildIds.Length);
            await InvokeEventAsync(e).ConfigureAwait(false);
            NotifyShardReady();
            return null;
        }

        shard.Logger.LogInformation("Identified as {0} with {1} pending guilds. Ready delay mode: {2}.", Dispatcher.CurrentUser.Tag, guildIds.Length, Dispatcher.ReadyEventDelayMode);
        _ = DelayReadyAsync(shard, e);
        return null;
    }

    /// <inheritdoc cref="DefaultGatewayDispatcher.WaitUntilReadyAsync"/>
    public Task WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        return _readyTcs.Task.WaitAsync(cancellationToken);
    }

    public bool IsPendingGuild(ShardId shardId, Snowflake guildId)
    {
        return PendingGuilds.TryGetValue(shardId, out var guilds) && guilds.ContainsKey(guildId);
    }

    public void PopPendingGuild(ShardId shardId, Snowflake guildId)
    {
        if (PendingGuilds.TryGetValue(shardId, out var guilds) && guilds.TryRemove(guildId, out _))
        {
            if (guilds.Count == 0 && _delays.TryGetValue(shardId, out var delay))
            {
                // Received all pending guilds, complete the delay.
                PendingGuilds.Remove(shardId);
                delay.Tcs.Complete();
            }
        }
    }

    private async Task DelayReadyAsync(IShard shard, ReadyEventArgs e)
    {
        if (_delays.TryGetValue(shard.Id, out var delay))
        {
            // If a disconnection happened cancel the already existing delay.
            delay.Tcs.Cancel();
            shard.Logger.LogWarning("The ready delay was overwritten by a new session.");
        }

        var sw = Stopwatch.StartNew();
        using (delay = new DelayToken(shard.StoppingToken))
        {
            _delays[shard.Id] = delay;
            try
            {
                await delay.Tcs.Task.ConfigureAwait(false);

                // The task finished meaning all pending guilds were received.
                // Now depending on the mode we either fire ready straight away or do chunking.
                _delays.Remove(shard.Id);
                switch (Dispatcher.ReadyEventDelayMode)
                {
                    case ReadyEventDelayMode.Guilds:
                    {
                        shard.Logger.LogInformation("Ready. Received all pending guilds in {0}ms.", sw.ElapsedMilliseconds);
                        await InvokeEventAsync(e).ConfigureAwait(false);
                        break;
                    }

                    // TODO: some chunking design...?
                }

                NotifyShardReady();
            }
            catch (OperationCanceledException)
            { }
        }
    }

    private readonly struct DelayToken : IDisposable
    {
        public Tcs Tcs { get; }

        private readonly CancellationTokenRegistration _reg;

        public DelayToken(CancellationToken stoppingToken)
        {
            Tcs = new Tcs();

            static void CancellationCallback(object? state, CancellationToken cancellationToken)
            {
                var tcs = Unsafe.As<Tcs>(state)!;
                tcs.Cancel(cancellationToken);
            }

            _reg = stoppingToken.UnsafeRegister(CancellationCallback, Tcs);
        }

        public void Dispose()
        {
            _reg.Dispose();
        }
    }
}
