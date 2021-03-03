using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Events;
using Disqord.Gateway;
using Disqord.Gateway.Api;
using Disqord.Rest;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Sharding
{
    // TODO: interface? shard states, management?
    public class DiscordClientSharder : DiscordClientBase
    {
        private readonly int? _configuredShardCount;
        private readonly ShardId[] _configuredShardIds;
        private readonly IShardFactory _shardFactory;
        private readonly IServiceProvider _services;
        private Dictionary<ShardId, IServiceScope> _scopes;

        public DiscordClientSharder(
            IOptions<DiscordClientSharderConfiguration> options,
            ILogger<DiscordClientSharder> logger,
            IRestClient restClient,
            IGatewayClient gatewayClient,
            IEnumerable<DiscordClientExtension> extensions,
            IShardFactory shardFactory,
            IServiceProvider services)
            : base(logger, restClient, gatewayClient, null, extensions)
        {
            var configuration = options.Value;
            _configuredShardCount = configuration.ShardCount;
            _configuredShardIds = configuration.ShardIds?.ToArray();
            _shardFactory = shardFactory;
            _services = services;
        }

        public override async Task RunAsync(CancellationToken stoppingToken)
        {
            if (GatewayClient.Shards is not ISynchronizedDictionary<ShardId, IGatewayApiClient> shards)
                throw new InvalidOperationException("The gateway client instance is expected to return a synchronized dictionary of shards.");

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
                var count = shardIds[0].Count;
                foreach (var id in _configuredShardIds)
                    shardIds.Add(id);
            }
            else
            {
                // TODO: get recommended shard count from Discord
                shardIds.Add(ShardId.Default);
            }

            Logger.LogInformation("This sharder will manage {0} shards with IDs: {1}", shardIds.Count, shardIds.Select(x => x.Id));

            // We create a service scope and a shard for every
            // shard ID this sharder is supposed to manage.
            foreach (var id in shardIds)
            {
                var scope = _services.CreateScope();
                _scopes.Add(id, scope);
                var shard = _shardFactory.Create(id, scope.ServiceProvider);
                shards.Add(id, shard);
            }

            {
                // We'll only start the default (first) shard, to test if everything it set up correctly.
                var readyTcs = new Tcs();
                var shard = shards[ShardId.Default];
                AsynchronousEventHandler<GatewayDispatchReceivedEventArgs> dispatchHandler = null;
                dispatchHandler = (sender, e) =>
                {
                    // Intercept READY early to complete the TCS.
                    if (e.Name == "READY")
                    {
                        readyTcs.Complete();
                        shard.DispatchReceived -= dispatchHandler;
                    }

                    return Task.CompletedTask;
                };
                shard.DispatchReceived += dispatchHandler;
                shard.DispatchReceived += GatewayClient.Dispatcher.HandleDispatchAsync;
                var runTask = shard.RunAsync(uri, stoppingToken);
                var task = Task.WhenAny(runTask, readyTcs.Task);
                try
                {
                    await task.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, "Terrible news overall");
                }

                foreach (var s in shards.Values.Skip(1))
                {
                    shard = s;
                    readyTcs = new Tcs();
                    shard.DispatchReceived += dispatchHandler;
                    shard.DispatchReceived += GatewayClient.Dispatcher.HandleDispatchAsync;
                    runTask = shard.RunAsync(uri, stoppingToken);
                    task = Task.WhenAny(runTask, readyTcs.Task);
                    try
                    {
                        await task.ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogCritical(ex, "Somewhat terrible news overall");
                    }
                }
            }
        }
    }
}
