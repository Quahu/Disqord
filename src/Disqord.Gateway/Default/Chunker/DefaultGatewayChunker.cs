using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Default;
using Disqord.Gateway.Api.Models;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon;
using Qommon.Binding;
using Qommon.Collections.ThreadSafe;

namespace Disqord.Gateway.Default;

/// <inheritdoc cref="IGatewayChunker"/>
public partial class DefaultGatewayChunker : IGatewayChunker, IAutomaticGatewayMemberChunker
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public IGatewayClient Client => _binder.Value;

    /// <summary>
    ///     Gets the time after which, if no chunk was received,
    ///     the chunk operations will time out and be retried.
    /// </summary>
    public TimeSpan OperationTimeout { get; }

    /// <summary>
    ///     Gets the maximum number of times a chunk operation is attempted before it is abandoned after repeatedly timing out.
    /// </summary>
    public int? MaxOperationAttempts { get; }

    private readonly IThreadSafeDictionary<string, ChunkOperation> _operations;

    private readonly Binder<IGatewayClient> _binder;

    /// <summary>
    ///     Instantiates a new <see cref="DefaultGatewayChunker"/>.
    /// </summary>
    /// <param name="options"> The chunker options. </param>
    /// <param name="shardOptions"> The shard options. </param>
    /// <param name="automaticChunkingSelector"> The automatic chunking guild selector. </param>
    /// <param name="logger"> The logger. </param>
    public DefaultGatewayChunker(
        IOptions<DefaultGatewayChunkerConfiguration> options,
        IOptions<DefaultShardConfiguration> shardOptions,
        IAutomaticGatewayMemberChunkingSelector automaticChunkingSelector,
        ILogger<DefaultGatewayChunker> logger)
    {
        var configuration = options.Value;
        Guard.IsGreaterThan(configuration.OperationTimeout, TimeSpan.Zero);

        if (configuration.MaxOperationAttempts is { } maxOperationAttempts)
        {
            Guard.IsGreaterThanOrEqualTo(maxOperationAttempts, 1);
        }

        var automaticChunking = configuration.AutomaticMemberChunking;
        if (automaticChunking.MaxConcurrentGuilds is { } maxConcurrentGuilds)
        {
            Guard.IsGreaterThanOrEqualTo(maxConcurrentGuilds, 1);
        }

        if (automaticChunking.IsEnabled && !shardOptions.Value.Intents.HasFlag(GatewayIntents.Members))
        {
            Throw.InvalidOperationException($"Automatic member chunking requires the {nameof(GatewayIntents.Members)} intent.");
        }

        OperationTimeout = configuration.OperationTimeout;
        MaxOperationAttempts = configuration.MaxOperationAttempts;
        Logger = logger;

        _isAutomaticChunkingEnabled = automaticChunking.IsEnabled;
        _chunkingStrategy = automaticChunking.Strategy;
        _chunkingSelector = automaticChunkingSelector;
        _maxConcurrentGuilds = automaticChunking.MaxConcurrentGuilds;

        _operations = ThreadSafeDictionary.Monitor.Create<string, ChunkOperation>();

        _binder = new Binder<IGatewayClient>(this);
    }

    /// <inheritdoc/>
    public void Bind(IGatewayClient value)
    {
        _binder.Bind(value);

        value.Dispatcher.GatewayRateLimitedEvent.Add(HandleRateLimited);
    }

    private Task HandleRateLimited(object? sender, GatewayRateLimitedEventArgs e)
    {
        if (e.Operation != GatewayPayloadOperation.RequestMembers)
        {
            return Task.CompletedTask;
        }

        var nonce = e.Metadata?["nonce"]?.ToType<string>();
        if (string.IsNullOrEmpty(nonce))
        {
            Logger.LogDebug("Received RATE_LIMITED with no nonce.");
            return Task.CompletedTask;
        }

        if (_operations.TryRemove(nonce, out var operation))
        {
            operation.Throw(new GatewayChunkerRateLimitedException(e.RetryAfter, e.Metadata));
        }

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public ValueTask OnChunk(GuildMembersChunkJsonModel model)
    {
        var members = new List<IMember>();
        try
        {
            if (model.Members.Length != 0)
            {
                if (Client.CacheProvider.TryGetUsers(out var userCache) && Client.CacheProvider.TryGetMembers(model.GuildId, out var memberCache))
                {
                    foreach (var memberModel in model.Members)
                    {
                        var member = Client.Dispatcher.GetOrAddMemberTransient(userCache, memberCache, model.GuildId, memberModel);
                        members.Add(member);
                    }
                }
                else
                {
                    foreach (var memberModel in model.Members)
                    {
                        var member = new TransientMember(Client, model.GuildId, memberModel);
                        members.Add(member);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to create members for received chunk.");
        }

        if (!model.Nonce.HasValue)
        {
            Logger.LogDebug("Received chunk with no nonce.");
            return default;
        }

        var isLastChunk = model.ChunkIndex == model.ChunkCount - 1;
        ChunkOperation? operation;
        if (isLastChunk)
        {
            if (!_operations.TryRemove(model.Nonce.Value, out operation))
            {
                return default;
            }
        }
        else
        {
            if (!_operations.TryGetValue(model.Nonce.Value, out operation))
            {
                return default;
            }
        }

        try
        {
            operation.OnChunk();
            if (operation.IsTimedOut)
            {
                return default;
            }

            operation.AddMembers(members);
            if (isLastChunk)
            {
                operation.Complete();
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to handle received chunk.");
            operation.Throw(ex);
        }

        return default;
    }

    /// <inheritdoc/>
    public ValueTask<bool> ChunkAsync(IGatewayGuild guild, CancellationToken cancellationToken = default)
    {
        return ChunkAsync(guild, null, cancellationToken);
    }

    /// <inheritdoc/>
    public async ValueTask<bool> ChunkAsync(IGatewayGuild guild, GatewayMemberChunkOptions? options, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(guild);
        ValidateOptions(options);

        if (!Client.CacheProvider.TryGetUsers(out _) || !Client.CacheProvider.TryGetMembers(guild.Id, out var memberCache))
        {
            return false;
        }

        if (memberCache.Count >= guild.MemberCount)
        {
            return false;
        }

        var model = new RequestMembersJsonModel
        {
            GuildId = guild.Id,
            Query = string.Empty,
            Limit = 0
        };

        await InternalOperationAsync(model, false, options?.MaxAttempts, cancellationToken).ConfigureAwait(false);
        return true;
    }

    /// <inheritdoc/>
    public ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, string query, int limit = Discord.Limits.Gateway.QueryMembersLimit, CancellationToken cancellationToken = default)
    {
        return QueryAsync(guildId, query, limit, null, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, string query, int limit, GatewayMemberChunkOptions? options, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(query);
        Guard.IsBetweenOrEqualTo(limit, 0, Discord.Limits.Gateway.QueryMembersLimit);
        ValidateOptions(options);

        var model = new RequestMembersJsonModel
        {
            GuildId = guildId,
            Query = query,
            Limit = limit
        };

        return InternalOperationAsync(model, true, options?.MaxAttempts, cancellationToken)!;
    }

    /// <inheritdoc/>
    public ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, IEnumerable<Snowflake> memberIds, CancellationToken cancellationToken = default)
    {
        return QueryAsync(guildId, memberIds, null, cancellationToken);
    }

    /// <inheritdoc/>
    public ValueTask<IReadOnlyDictionary<Snowflake, IMember>> QueryAsync(Snowflake guildId, IEnumerable<Snowflake> memberIds, GatewayMemberChunkOptions? options, CancellationToken cancellationToken = default)
    {
        var ids = memberIds.ToArray();
        Guard.HasSizeGreaterThanOrEqualTo(ids, 1, nameof(memberIds));
        Guard.HasSizeLessThanOrEqualTo(ids, Discord.Limits.Gateway.QueryMembersLimit, nameof(memberIds));
        ValidateOptions(options);

        var model = new RequestMembersJsonModel
        {
            GuildId = guildId,

            // Since the gateway is clowning around and not sending back the nonce for small limits
            // or whatever, let's just ask for the max limit always.
            Limit = Discord.Limits.Gateway.QueryMembersLimit,
            UserIds = ids
        };

        return InternalOperationAsync(model, true, options?.MaxAttempts, cancellationToken)!;
    }

    private static void ValidateOptions(GatewayMemberChunkOptions? options)
    {
        if (options?.MaxAttempts is { } maxAttempts)
        {
            Guard.IsGreaterThanOrEqualTo(maxAttempts, 1);
        }
    }

    private async ValueTask<IReadOnlyDictionary<Snowflake, IMember>?> InternalOperationAsync(RequestMembersJsonModel model, bool isQuery, int? maxAttempts, CancellationToken cancellationToken)
    {
        var maxOperationAttempts = maxAttempts ?? MaxOperationAttempts;
        var attempt = 0;
        while (true)
        {
            var operation = new ChunkOperation(OperationTimeout, isQuery, cancellationToken);
            model.Nonce = operation.Nonce;
            model.Presences = true; // According to the docs should default to false without the presences intent.
            var shard = Client.ApiClient.GetShard(model.GuildId);
            if (shard == null)
            {
                operation.Dispose();
                Throw.InvalidOperationException("Failed to get the shard of the guild.");
            }

            _operations.Add(operation.Nonce, operation);
            try
            {
                await shard.SendAsync(new GatewayPayloadJsonModel
                {
                    Op = GatewayPayloadOperation.RequestMembers,
                    D = model
                }, cancellationToken).ConfigureAwait(false);

                return await operation.WaitAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                if (ex is TimeoutException)
                {
                    attempt++;
                    if (maxOperationAttempts == null || attempt < maxOperationAttempts)
                    {
                        shard.Logger.LogWarning("The chunk operation for guild {0} timed out after {1}ms. Retrying...", model.GuildId.RawValue, (int) OperationTimeout.TotalMilliseconds);
                        continue;
                    }
                }

                throw;
            }
            finally
            {
                _operations.Remove(operation.Nonce);
                operation.Dispose();
            }
        }
    }

    private sealed class ChunkOperation : IDisposable
    {
        public string Nonce { get; }

        public bool IsTimedOut => _timeoutCts.IsCancellationRequested;

        private readonly TimeSpan _timeout;
        private readonly Cts _timeoutCts;
        private readonly Dictionary<Snowflake, IMember>? _members;
        private readonly Tcs<IReadOnlyDictionary<Snowflake, IMember>?> _tcs;
        private readonly CancellationTokenRegistration _reg;
        private readonly CancellationTokenRegistration _timeoutReg;

        public ChunkOperation(TimeSpan timeout, bool isQuery, CancellationToken cancellationToken)
        {
            Nonce = Guid.NewGuid().ToString("N");
            _timeout = timeout;
            _timeoutCts = new Cts();

            _members = isQuery
                ? new Dictionary<Snowflake, IMember>()
                : null;

            _tcs = new Tcs<IReadOnlyDictionary<Snowflake, IMember>?>();

            _reg = cancellationToken.UnsafeRegister(CancellationCallback, _tcs);
            _timeoutReg = _timeoutCts.Token.UnsafeRegister(TimeoutCallback, _tcs);
            return;

            static void CancellationCallback(object? state, CancellationToken cancellationToken)
            {
                var tcs = Unsafe.As<Tcs<IReadOnlyDictionary<Snowflake, IMember>?>>(state)!;
                tcs.Cancel(cancellationToken);
            }

            static void TimeoutCallback(object? state, CancellationToken cancellationToken)
            {
                var tcs = Unsafe.As<Tcs<IReadOnlyDictionary<Snowflake, IMember>?>>(state)!;
                tcs.Throw(new TimeoutException());
            }
        }

        public Task<IReadOnlyDictionary<Snowflake, IMember>?> WaitAsync()
        {
            _timeoutCts.CancelAfter(_timeout);
            return _tcs.Task;
        }

        public void OnChunk()
        {
            _timeoutCts.CancelAfter(_timeout);
        }

        public void AddMembers(IReadOnlyList<IMember> members)
        {
            lock (this)
            {
                if (_members == null)
                    return;

                for (var i = 0; i < members.Count; i++)
                {
                    var member = members[i];
                    _members.Add(member.Id, member);
                }
            }
        }

        public void Complete()
        {
            lock (this)
            {
                if (_members == null)
                {
                    _tcs.Complete(null);
                    return;
                }

                _tcs.Complete(_members);
            }
        }

        public void Throw(Exception exception)
        {
            lock (this)
            {
                _tcs.Throw(exception);
            }
        }

        public void Dispose()
        {
            _reg.Dispose();
            _timeoutReg.Dispose();
            _timeoutCts.Dispose();
        }
    }
}
