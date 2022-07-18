using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Gateway.Default;
using Disqord.Gateway.Default.Dispatcher;
using Disqord.Rest;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Collections.Synchronized;

namespace Disqord.Sharding;

// TODO: interface? shard states, management?
public class DiscordClientSharder : DiscordClientBase
{
    private Dictionary<ShardId, IServiceScope>? _scopes;

    private readonly int? _configuredShardCount;
    private readonly ShardId[]? _configuredShardIds;
    private readonly IShardFactory _shardFactory;
    private readonly IServiceProvider _services;

    private readonly Tcs _initialReadyTcs;

    public DiscordClientSharder(
        IOptions<DiscordClientSharderConfiguration> options,
        ILogger<DiscordClientSharder> logger,
        IRestClient restClient,
        IGatewayClient gatewayClient,
        IEnumerable<DiscordClientExtension> extensions,
        IShardFactory shardFactory,
        IServiceProvider services)
        : base(logger, restClient, gatewayClient, extensions)
    {
        if (GatewayClient.Shards is not ISynchronizedDictionary<ShardId, IGatewayApiClient>)
            throw new InvalidOperationException("The gateway client instance is expected to return a synchronized dictionary of shards.");

        if (GatewayClient.Dispatcher is not DefaultGatewayDispatcher dispatcher || dispatcher["READY"] is not ReadyDispatchHandler)
            throw new InvalidOperationException("The gateway dispatcher must be the default implementation.");

        var configuration = options.Value;
        _configuredShardCount = configuration.ShardCount;
        _configuredShardIds = configuration.ShardIds?.ToArray();
        _shardFactory = shardFactory;
        _services = services;

        _initialReadyTcs = new Tcs();
    }

    public override async Task RunAsync(CancellationToken stoppingToken)
    {
        StoppingToken = stoppingToken;
        var uri = new Uri("wss://gateway.discord.gg/");
        _scopes = new Dictionary<ShardId, IServiceScope>();
        var shardIds = new List<ShardId>();
        if (_configuredShardCount != null)
        {
            // If the shard count is specified we generate the appropriate shard IDs.
            var shardCount = _configuredShardCount.Value;
            for (var i = 0; i < shardCount; i++)
            {
                var id = new ShardId(i, shardCount);
                shardIds.Add(id);
            }
        }
        else if (_configuredShardIds != null)
        {
            // If the shard IDs are specified we validate them and manage those instead.
            // TODO: validation
            foreach (var id in _configuredShardIds)
                shardIds.Add(id);
        }
        else
        {
            var botGatewayData = await this.FetchBotGatewayDataAsync(cancellationToken: stoppingToken).ConfigureAwait(false);
            Logger.LogDebug("Using Discord's recommended shard count of {0}.", botGatewayData.RecommendedShardCount);
            for (var i = 0; i < botGatewayData.RecommendedShardCount; i++)
            {
                var id = new ShardId(i, botGatewayData.RecommendedShardCount);
                shardIds.Add(id);
            }
        }

        Logger.LogInformation("This sharder will manage {0} shards with IDs: {1}", shardIds.Count, shardIds.Select(x => x.Id));
        var shards = (GatewayClient.Shards as ISynchronizedDictionary<ShardId, IGatewayApiClient>)!;
        var dispatcher = (GatewayClient.Dispatcher as DefaultGatewayDispatcher)!;
        var originalReadyHandler = (dispatcher["READY"] as ReadyDispatchHandler)!;
        originalReadyHandler.InitialReadys.Clear();

        // We create a service scope and a shard for every
        // shard ID this sharder is supposed to manage.
        foreach (var id in shardIds)
        {
            var scope = _services.CreateScope();
            _scopes.Add(id, scope);
            shards.Add(id, _shardFactory.Create(id, scope.ServiceProvider));

            originalReadyHandler.InitialReadys.Add(id, new Tcs());
        }

        _initialReadyTcs.Complete();

        // These two locals will be reused via the closure below.
        Tcs? readyTcs = null;
        ShardId shardId = default;

        dispatcher["READY"] = originalReadyHandler.Intercept((shard, _) =>
        {
            if (shard.Id == shardId)
            {
                // If the shard that identified is the shard ID we're booting up
                // we complete the TCS to signal the code below to boot the next shard.
                readyTcs!.Complete();
            }
        });

        var linkedCts = Cts.Linked(stoppingToken);
        var runTasks = new List<Task>(shards.Count);
        foreach (var shard in shards.Values)
        {
            // Set the TCS and shard ID for the shard we'll boot up
            // which are used by the intercepting ready handler above.
            readyTcs = new Tcs();
            shardId = shard.Id;

            shard.DispatchReceived += dispatcher.HandleDispatchAsync;
            var runTask = shard.RunAsync(uri, linkedCts.Token);
            var task = Task.WhenAny(runTask, readyTcs.Task);
            try
            {
                if (await task.ConfigureAwait(false) == runTask)
                {
                    // If the task that finished is the run task
                    // it means an exception occurred when identifying.
                    // We await it to get the exception thrown.
                    await runTask.ConfigureAwait(false);
                }

                // If we reached here it means the shard successfully identified,
                // so we add the task to the list and continue.
                runTasks.Add(runTask);
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "An exception occurred while starting {0}.", shardId);

                // Cancel the CTS so that if some shards are running already
                // it's a coordinated close as they will probably break sooner or later.
                linkedCts.Cancel();

                // We bubble the exception up to the client runner.
                throw;
            }
        }

        shardId = default;
        try
        {
            // We wait for any of the tasks to finish (throw)
            // and handle it appropriately.
            var runTask = await Task.WhenAny(runTasks).ConfigureAwait(false);
            await runTask.ConfigureAwait(false);
        }
        catch
        {
            // TODO: if sharding required redo the process with more shards blah blah
            linkedCts.Cancel();
            throw;
        }
    }

    public override async Task WaitUntilReadyAsync(CancellationToken cancellationToken)
    {
        var tcs = new Tcs();

        static void CancellationCallback(object? state, CancellationToken cancellationToken)
        {
            var tcs = Unsafe.As<Tcs>(state)!;
            tcs.Cancel(cancellationToken);
        }

        await using (var reg = cancellationToken.UnsafeRegister(CancellationCallback, tcs))
        {
            // TODO: do something on ready closure
            var task = await Task.WhenAny(InternalWaitUntilReadyAsync(), tcs.Task).ConfigureAwait(false);
            await task.ConfigureAwait(false);
        }
    }

    private async Task InternalWaitUntilReadyAsync()
    {
        // Waits for RunAsync() to populate the TCS dictionary.
        await _initialReadyTcs.Task.ConfigureAwait(false);

        var dispatcher = (GatewayClient.Dispatcher as DefaultGatewayDispatcher)!;
        var handler = dispatcher["READY"];
        var readyHandler = (handler is InterceptingDispatchHandler<ReadyJsonModel, ReadyEventArgs> interceptingHandler
            ? interceptingHandler.UnderlyingDispatchHandler as ReadyDispatchHandler
            : handler as ReadyDispatchHandler)!;

        await Task.WhenAll(readyHandler.InitialReadys.Values.Select(x => x.Task)).ConfigureAwait(false);
    }
}
