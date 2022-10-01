using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Serialization.Json;
using Disqord.Utilities.Threading;
using Disqord.WebSocket;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon;
using Qommon.Collections.ReadOnly;
using Qommon.Collections.Synchronized;
using Qommon.Events;

namespace Disqord.Gateway.Api.Default;

public class DefaultGatewayApiClient : IGatewayApiClient
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public Token Token { get; }

    /// <inheritdoc/>
    public IJsonSerializer Serializer { get; }

    /// <inheritdoc/>
    public IShardCoordinator ShardCoordinator { get; }

    /// <inheritdoc/>
    public IShardFactory ShardFactory { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<ShardId, IShard> Shards => _shards as IReadOnlyDictionary<ShardId, IShard> ?? ReadOnlyDictionary<ShardId, IShard>.Empty;

    /// <inheritdoc/>
    public CancellationToken StoppingToken { get; private set; }

    /// <inheritdoc/>
    public AsynchronousEvent<GatewayDispatchReceivedEventArgs> DispatchReceivedEvent { get; }

    private ISynchronizedDictionary<ShardId, IShard>? _shards;

    public DefaultGatewayApiClient(
        IOptions<DefaultGatewayApiClientConfiguration> options,
        ILogger<DefaultGatewayApiClient> logger,
        Token token,
        IJsonSerializer serializer,
        IShardCoordinator shardCoordinator,
        IShardFactory shardFactory)
    {
        Logger = logger;
        Serializer = serializer;
        Token = token;
        ShardCoordinator = shardCoordinator;
        ShardFactory = shardFactory;
        DispatchReceivedEvent = new();
    }

    public async Task RunAsync(Uri? initialUri, CancellationToken stoppingToken)
    {
        StoppingToken = stoppingToken;

        var shardSetAttempt = 0;
        while (!stoppingToken.IsCancellationRequested)
        {
            if (shardSetAttempt != 0)
            {
                if (shardSetAttempt > 1)
                {
                    Throw.InvalidOperationException($"The shard set was invalid {shardSetAttempt + 1} times in a row. "
                        + "Validate the shard set provider logic.");
                }

                Logger.LogInformation("Reinitializing the shards...");
            }

            var shardQueues = await InitializeShards(stoppingToken).ConfigureAwait(false);
            var shards = _shards!.Values;
            Logger.LogInformation("Running {ShardCount} shards with indices: {ShardIndices}...", shards.Length, shards.Select(shard => shard.Id.Index));
            var linkedCts = Cts.Linked(stoppingToken);
            var runTasks = new Task[shards.Length];
            var runTaskIndex = 0;
            try
            {
                var shardBucket = new List<IShard>();
                var reset = false;
                while (!stoppingToken.IsCancellationRequested)
                {
                    shardBucket.Clear();
                    for (var i = 0; i < shardQueues.Length; i++)
                    {
                        if (!shardQueues[i].MoveNext())
                            continue;

                        shardBucket.Add(shardQueues[i].Current);
                    }

                    if (shardBucket.Count == 0)
                        break;

                    var bucketTasks = new Task<Task?>[shardBucket.Count];
                    for (var i = 0; i < shardBucket.Count; i++)
                    {
                        var shard = shardBucket[i];
                        var linkedCancellationToken = linkedCts.Token;
                        bucketTasks[i] = Task.Run(async () =>
                        {
                            var readyTask = shard.WaitForReadyAsync(linkedCancellationToken);
                            var runTask = shard.RunAsync(initialUri, linkedCancellationToken);
                            var completedTask = await Task.WhenAny(readyTask, runTask).ConfigureAwait(false);
                            if (completedTask == runTask)
                            {
                                if (runTask.Exception == null)
                                    return null;

                                Debug.Assert(runTask.Exception.InnerException != null);

                                if (!ShardCoordinator.HasDynamicShardSets
                                    || runTask.Exception.InnerException is not WebSocketClosedException webSocketClosedException
                                    || webSocketClosedException.CloseStatus == null
                                    || (GatewayCloseCode) webSocketClosedException.CloseStatus.Value != GatewayCloseCode.ShardingRequired)
                                {
                                    // Ignore all but a sharding required close code
                                    // when the coordinator has dynamic shard sets.
                                    ExceptionDispatchInfo.Capture(runTask.Exception).Throw();
                                }

                                return null;
                            }

                            return runTask;
                        }, stoppingToken);
                    }

                    await Task.WhenAll(bucketTasks).ConfigureAwait(false);
                    List<Exception>? exceptions = null;
                    foreach (var bucketTask in bucketTasks)
                    {
                        if (bucketTask.Exception != null)
                            (exceptions ??= new()).Add(bucketTask.Exception);
                    }

                    if (exceptions != null)
                    {
                        linkedCts.Cancel();
                        throw new AggregateException(exceptions);
                    }

                    foreach (var bucketTask in bucketTasks)
                    {
                        if (bucketTask.Result == null)
                        {
                            reset = true;
                            break;
                        }

                        runTasks[runTaskIndex++] = bucketTask.Result;
                    }

                    if (reset)
                        break;
                }

                if (reset)
                {
                    linkedCts.Cancel();
                    shardSetAttempt++;
                    continue;
                }

                shardSetAttempt = 0;
                await Task.WhenAll(runTasks).ConfigureAwait(false);
            }
            finally
            {
                await Task.WhenAll(runTasks.Take(runTaskIndex)).ConfigureAwait(false);

                foreach (var shard in shards)
                {
                    try
                    {
                        await shard.DisposeAsync().ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "An exception occurred while disposing {ShardId}.", shard.Id);
                    }
                }
            }
        }
    }

    protected async ValueTask<List<IShard>.Enumerator[]> InitializeShards(CancellationToken stoppingToken)
    {
        var shardSet = await ShardCoordinator.GetShardSetAsync(stoppingToken).ConfigureAwait(false);
        var shards = new Dictionary<ShardId, IShard>();

        var shardGroups = shardSet.ShardIds.GroupBy(shardId => shardId.Index % shardSet.MaxConcurrency).ToArray();
        Array.Sort(shardGroups, (a, b) => a.Key.CompareTo(b.Key));

        var shardQueues = new List<IShard>.Enumerator[shardGroups.Length];
        for (var i = 0; i < shardGroups.Length; i++)
        {
            var shardGroup = shardGroups[i];
            var shardQueue = new List<IShard>();
            foreach (var shardId in shardGroup)
            {
                var shard = ShardFactory.Create(shardId);
                shards[shardId] = shard;
                shardQueue.Add(shard);
            }

            shardQueues[i] = shardQueue.GetEnumerator();
        }

        _shards = new SynchronizedDictionary<ShardId, IShard>(shards);
        return shardQueues;
    }
}
