using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Gateway.Api;

namespace Disqord.Gateway
{
    public partial interface IGatewayClient : IClient
    {
        IGatewayCacheProvider CacheProvider { get; }

        IGatewayDispatcher Dispatcher { get; }

        /// <summary>
        ///     Gets the shards managed by this <see cref="IGatewayApiClient"/>.
        /// </summary>
        IReadOnlyDictionary<ShardId, IGatewayApiClient> Shards { get; }

        /// <summary>
        ///     Gets the low-level version of this client.
        ///     If this client manages multiple shards this will return the shard for <see cref="ShardId.Default"/>.
        /// </summary>
        /// <remarks>
        ///     Do not use this unless you are well aware of how it works.
        /// </remarks>
        new IGatewayApiClient ApiClient => Shards.GetValueOrDefault(ShardId.Default);

        IApiClient IClient.ApiClient => ApiClient;

        ICurrentUser CurrentUser => Dispatcher.CurrentUser;

        /// <summary>
        ///     Runs this <see cref="IGatewayClient"/>.
        /// </summary>
        /// <param name="uri"> The Discord gateway URI to connect to. </param>
        /// <param name="stoppingToken"> The token used to signal connection stopping. </param>
        /// <returns> The <see cref="Task"/> representing the connection. </returns>
        Task RunAsync(Uri uri, CancellationToken stoppingToken);

        ShardId GetShardId(Snowflake? guildId)
            => ShardId.ForGuildId(guildId ?? 0, Shards.Count);

        IGatewayApiClient GetShard(Snowflake? guildId)
            => Shards.GetValueOrDefault(GetShardId(guildId));
    }
}
