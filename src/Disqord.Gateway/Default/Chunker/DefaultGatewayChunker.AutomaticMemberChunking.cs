using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Qommon;

namespace Disqord.Gateway.Default;

public partial class DefaultGatewayChunker
{
    private const int MaxRateLimitedAttempts = 3;

    private readonly bool _isAutomaticChunkingEnabled;
    private readonly AutomaticMemberChunkingStrategy _chunkingStrategy;
    private readonly IAutomaticGatewayMemberChunkingSelector _chunkingSelector;
    private readonly int? _maxConcurrentGuilds;

    private readonly object _automaticChunkingLock = new();
    private readonly Dictionary<ShardId, ShardChunkingSession> _chunkingSessions = [];
    private readonly Dictionary<ShardId, SemaphoreSlim> _chunkingSemaphores = [];

    bool IAutomaticGatewayMemberChunker.IsEnabled => _isAutomaticChunkingEnabled;

    Task IAutomaticGatewayMemberChunker.OnReady(IShard shard, IReadOnlyList<Snowflake> guildIds)
    {
        ShardChunkingSession? previousSession;
        lock (_automaticChunkingLock)
        {
            _chunkingSessions.Remove(shard.Id, out previousSession);
        }

        if (previousSession != null)
        {
            if (!previousSession.Tcs.Task.IsCompleted)
            {
                shard.Logger.LogDebug("The chunking of the pending guilds was cancelled by a new session.");
            }

            previousSession.Tcs.Cancel();
            previousSession.Cts.Cancel();
        }

        if (!_isAutomaticChunkingEnabled || guildIds.Count == 0)
        {
            return Task.CompletedTask;
        }

        var session = new ShardChunkingSession(this, shard, guildIds);
        lock (_automaticChunkingLock)
        {
            _chunkingSessions[shard.Id] = session;
        }

        return session.Tcs.Task;
    }

    void IAutomaticGatewayMemberChunker.OnGuildAvailable(IShard shard, IGatewayGuild guild, bool isPending)
    {
        if (!_isAutomaticChunkingEnabled)
        {
            return;
        }

        try
        {
            ShardChunkingSession? session;
            lock (_automaticChunkingLock)
            {
                session = _chunkingSessions.GetValueOrDefault(shard.Id);
            }

            if (isPending && _chunkingStrategy == AutomaticMemberChunkingStrategy.WhenAllAvailable)
            {
                if (session == null)
                {
                    return;
                }

                lock (_automaticChunkingLock)
                {
                    session.Guilds.Add(guild);
                }

                return;
            }

            if (!ShouldChunk(shard, guild))
            {
                return;
            }

            if (isPending && session != null)
            {
                var task = ChunkSelectedPendingGuildAsync(session, guild);
                lock (_automaticChunkingLock)
                {
                    if (session.ChunkTasks.Count == 0)
                    {
                        session.ChunkingStartTimestamp = Stopwatch.GetTimestamp();
                    }

                    session.ChunkTasks.Add(task);
                }
            }
            else
            {
                _ = ChunkAvailableGuildAsync(shard, guild, session?.Cts.Token ?? shard.StoppingToken);
            }
        }
        catch (Exception ex)
        {
            shard.Logger.LogError(ex, "Failed to chunk guild {0} ({1}).", guild.Name, guild.Id.RawValue);
        }
    }

    void IAutomaticGatewayMemberChunker.OnPendingGuildsReceived(ShardId shardId)
    {
        if (!_isAutomaticChunkingEnabled)
        {
            return;
        }

        ShardChunkingSession session;
        lock (_automaticChunkingLock)
        {
            if (!_chunkingSessions.TryGetValue(shardId, out var trackedSession) || trackedSession.IsChunking)
            {
                return;
            }

            trackedSession.IsChunking = true;
            session = trackedSession;
        }

        _ = ChunkPendingGuildsAsync(session);
    }

    private async Task<int> ChunkSelectedPendingGuildAsync(ShardChunkingSession session, IGatewayGuild guild)
    {
        var shard = session.Shard;
        try
        {
            var selection = await session.SelectionTask.ConfigureAwait(false);
            if (!selection.IsSelected(guild.Id))
            {
                shard.Logger.LogDebug("The selector excluded guild {0} ({1}) from chunking.", guild.Name, guild.Id.RawValue);
                return 0;
            }

            return await AutomaticallyChunkAsync(shard, [guild], session.Cts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            return 0;
        }
    }

    private async Task ChunkAvailableGuildAsync(IShard shard, IGatewayGuild guild, CancellationToken cancellationToken)
    {
        try
        {
            var guildIds = GetShardGuildIds(shard, guild);
            var selection = await SelectGuildsAsync(shard, guildIds, cancellationToken).ConfigureAwait(false);
            if (!selection.IsSelected(guild.Id))
            {
                shard.Logger.LogDebug("The selector excluded guild {0} ({1}) from chunking.", guild.Name, guild.Id.RawValue);
                return;
            }

            await AutomaticallyChunkAsync(shard, [guild], cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            shard.Logger.LogDebug("The chunking of guild {0} ({1}) was cancelled.", guild.Name, guild.Id.RawValue);
        }
        catch (Exception ex)
        {
            shard.Logger.LogError(ex, "Failed to chunk guild {0} ({1}).", guild.Name, guild.Id.RawValue);
        }
    }

    private IReadOnlyList<Snowflake> GetShardGuildIds(IShard shard, IGatewayGuild guild)
    {
        var guilds = Client.GetGuilds();
        var shardId = shard.Id;
        var guildIds = new List<Snowflake>(guilds.Count);
        foreach (var guildId in guilds.Keys)
        {
            if (!shardId.HasCount || ShardId.FromGuildId(guildId, shardId.Count) == shardId)
            {
                guildIds.Add(guildId);
            }
        }

        if (!guildIds.Contains(guild.Id))
        {
            guildIds.Add(guild.Id);
        }

        return guildIds;
    }

    private bool ShouldChunk(IShard shard, IGatewayGuild guild)
    {
        if (IsGuildChunked(guild))
        {
            shard.Logger.LogDebug("Not chunking guild {0} ({1}) as all of its members are cached.", guild.Name, guild.Id.RawValue);
            return false;
        }

        return true;
    }

    private bool IsGuildChunked(IGatewayGuild guild)
    {
        return GetCachedMemberCount(guild) >= guild.MemberCount;
    }

    private int GetCachedMemberCount(IGatewayGuild guild)
    {
        return Client.CacheProvider.TryGetMembers(guild.Id, out var memberCache)
            ? memberCache.Count
            : 0;
    }

    private async Task ChunkPendingGuildsAsync(ShardChunkingSession session)
    {
        try
        {
            if (_chunkingStrategy == AutomaticMemberChunkingStrategy.AsAvailable)
            {
                await WaitForPendingGuildChunkTasksAsync(session).ConfigureAwait(false);
            }
            else
            {
                await ChunkTrackedPendingGuildsAsync(session).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            session.Shard.Logger.LogError(ex, "Failed to chunk the pending guilds.");
        }
        finally
        {
            session.Tcs.Complete();
        }
    }

    private async Task WaitForPendingGuildChunkTasksAsync(ShardChunkingSession session)
    {
        Task<int>[] tasks;
        lock (_automaticChunkingLock)
        {
            tasks = session.ChunkTasks.ToArray();
            session.ChunkTasks.Clear();
        }

        if (tasks.Length == 0)
        {
            session.Shard.Logger.LogDebug("No pending guilds required chunking.");
            return;
        }

        var chunkedCounts = await Task.WhenAll(tasks).ConfigureAwait(false);
        LogChunkedPendingGuilds(session, chunkedCounts.Sum());
    }

    private async Task ChunkTrackedPendingGuildsAsync(ShardChunkingSession session)
    {
        IGatewayGuild[] guilds;
        lock (_automaticChunkingLock)
        {
            guilds = session.Guilds.ToArray();
            session.Guilds.Clear();
        }

        var shard = session.Shard;
        var candidates = new List<IGatewayGuild>(guilds.Length);
        foreach (var guild in guilds)
        {
            if (ShouldChunk(shard, guild))
            {
                candidates.Add(guild);
            }
        }

        if (candidates.Count == 0)
        {
            shard.Logger.LogDebug("No pending guilds required chunking.");
            return;
        }

        var selection = await session.SelectionTask.ConfigureAwait(false);
        var selected = SelectCandidates(candidates, selection);
        if (selected.Count == 0)
        {
            shard.Logger.LogDebug("The selector excluded all pending guilds from chunking.");
            return;
        }

        shard.Logger.LogInformation("Chunking {0} of {1} pending guilds...", selected.Count, guilds.Length);
        session.ChunkingStartTimestamp = Stopwatch.GetTimestamp();
        var chunkedCount = await AutomaticallyChunkAsync(shard, selected, session.Cts.Token).ConfigureAwait(false);
        LogChunkedPendingGuilds(session, chunkedCount);
    }

    private static IReadOnlyList<IGatewayGuild> SelectCandidates(IReadOnlyList<IGatewayGuild> candidates, ChunkSelection selection)
    {
        if (selection.OrderedIds == null)
        {
            return candidates;
        }

        var candidatesById = new Dictionary<Snowflake, IGatewayGuild>(candidates.Count);
        foreach (var candidate in candidates)
        {
            candidatesById[candidate.Id] = candidate;
        }

        var selectedGuilds = new List<IGatewayGuild>(candidates.Count);
        foreach (var guildId in selection.OrderedIds)
        {
            if (candidatesById.TryGetValue(guildId, out var candidate))
            {
                selectedGuilds.Add(candidate);
            }
        }

        return selectedGuilds;
    }

    private static void LogChunkedPendingGuilds(ShardChunkingSession session, int chunkedCount)
    {
        if (session.Cts.IsCancellationRequested)
        {
            return;
        }

        if (chunkedCount == 0)
        {
            session.Shard.Logger.LogDebug("No pending guilds were chunked.");
            return;
        }

        session.Shard.Logger.LogInformation("Chunked {0} pending guilds in {1}ms.", chunkedCount, (int) Stopwatch.GetElapsedTime(session.ChunkingStartTimestamp).TotalMilliseconds);
    }

    private async Task<ChunkSelection> SelectGuildsAsync(IShard shard, IReadOnlyList<Snowflake> guildIds, CancellationToken cancellationToken)
    {
        try
        {
            var selectedIds = await _chunkingSelector.SelectAsync(shard, guildIds, cancellationToken).ConfigureAwait(false);
            Guard.IsNotNull(selectedIds);
            return ChunkSelection.From(selectedIds);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            shard.Logger.LogError(ex, "The selector failed. All guilds will be chunked.");
            return ChunkSelection.All;
        }
    }

    private async Task<int> AutomaticallyChunkAsync(IShard shard, IReadOnlyList<IGatewayGuild> guilds, CancellationToken cancellationToken)
    {
        using var cts = Cts.Linked(cancellationToken, shard.StoppingToken);
        var linkedToken = cts.Token;
        var semaphore = GetChunkingSemaphore(shard.Id);
        var tasks = new List<Task<bool>>(guilds.Count);
        foreach (var guild in guilds)
        {
            if (semaphore != null)
            {
                try
                {
                    await semaphore.WaitAsync(linkedToken).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            tasks.Add(AutomaticallyChunkGuildAsync(shard, guild, semaphore, linkedToken));
        }

        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results.Count(static wasChunked => wasChunked);
    }

    private SemaphoreSlim? GetChunkingSemaphore(ShardId shardId)
    {
        if (_maxConcurrentGuilds is not { } maxConcurrentGuilds)
        {
            return null;
        }

        lock (_automaticChunkingLock)
        {
            if (!_chunkingSemaphores.TryGetValue(shardId, out var semaphore))
            {
                semaphore = new SemaphoreSlim(maxConcurrentGuilds, maxConcurrentGuilds);
                _chunkingSemaphores[shardId] = semaphore;
            }

            return semaphore;
        }
    }

    private async Task<bool> AutomaticallyChunkGuildAsync(IShard shard, IGatewayGuild guild, SemaphoreSlim? semaphore, CancellationToken cancellationToken)
    {
        var logger = shard.Logger;
        try
        {
            logger.LogDebug("Chunking guild {0} ({1}) with {2} of {3} members cached...", guild.Name, guild.Id.RawValue, GetCachedMemberCount(guild), guild.MemberCount);
            var startTimestamp = Stopwatch.GetTimestamp();
            var attempt = 0;
            while (true)
            {
                try
                {
                    var wasChunked = await ChunkAsync(guild, cancellationToken).ConfigureAwait(false);
                    if (wasChunked)
                    {
                        logger.LogDebug("Chunked guild {0} ({1}) in {2}ms.", guild.Name, guild.Id.RawValue, (int) Stopwatch.GetElapsedTime(startTimestamp).TotalMilliseconds);
                    }
                    else if (IsGuildChunked(guild))
                    {
                        logger.LogDebug("Guild {0} ({1}) was not chunked as all of its members are cached.", guild.Name, guild.Id.RawValue);
                    }
                    else
                    {
                        logger.LogDebug("Guild {0} ({1}) was not chunked as it has no member cache.", guild.Name, guild.Id.RawValue);
                    }

                    return wasChunked;
                }
                catch (GatewayChunkerRateLimitedException ex)
                {
                    if (++attempt == MaxRateLimitedAttempts)
                    {
                        throw;
                    }

                    logger.LogWarning("Hit the chunk rate-limit for guild {0} ({1}), retrying in {2}ms...", guild.Name, guild.Id.RawValue, (int) ex.RetryAfter.TotalMilliseconds);
                    await Task.Delay(ex.RetryAfter, cancellationToken).ConfigureAwait(false);
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogDebug("The chunking of guild {0} ({1}) was cancelled.", guild.Name, guild.Id.RawValue);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to chunk guild {0} ({1}).", guild.Name, guild.Id.RawValue);
        }
        finally
        {
            semaphore?.Release();
        }

        return false;
    }

    private sealed class ShardChunkingSession
    {
        public IShard Shard { get; }

        public Cts Cts { get; }

        public Task<ChunkSelection> SelectionTask { get; }

        public List<IGatewayGuild> Guilds { get; }

        public List<Task<int>> ChunkTasks { get; }

        public Tcs Tcs { get; }

        public long ChunkingStartTimestamp { get; set; }

        public bool IsChunking { get; set; }

        public ShardChunkingSession(DefaultGatewayChunker chunker, IShard shard, IReadOnlyList<Snowflake> guildIds)
        {
            Shard = shard;
            Cts = new Cts();
            SelectionTask = chunker.SelectGuildsAsync(shard, guildIds, Cts.Token);
            Guilds = [];
            ChunkTasks = [];
            Tcs = new Tcs();
        }
    }

    private sealed class ChunkSelection
    {
        public static readonly ChunkSelection All = new(null, null);

        private readonly HashSet<Snowflake>? _ids;

        public IReadOnlyList<Snowflake>? OrderedIds { get; }

        private ChunkSelection(HashSet<Snowflake>? ids, IReadOnlyList<Snowflake>? orderedIds)
        {
            _ids = ids;
            OrderedIds = orderedIds;
        }

        public bool IsSelected(Snowflake guildId)
        {
            return _ids == null || _ids.Contains(guildId);
        }

        public static ChunkSelection From(IEnumerable<Snowflake> selectedIds)
        {
            var ids = new HashSet<Snowflake>();
            var orderedIds = new List<Snowflake>();
            foreach (var selectedId in selectedIds)
            {
                if (ids.Add(selectedId))
                {
                    orderedIds.Add(selectedId);
                }
            }

            return new ChunkSelection(ids, orderedIds);
        }
    }
}
