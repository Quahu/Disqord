using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest;

namespace Disqord.Sharding
{
    public partial class DiscordSharder : DiscordClientBase, IDiscordSharder
    {
        public IReadOnlyList<Shard> Shards { get; private set; }

        private readonly int _shardCount;

        public DiscordSharder(TokenType tokenType, string token, DiscordSharderConfiguration configuration = null)
            : this(new RestDiscordClient(tokenType, token, configuration), configuration)
        { }

        public DiscordSharder(RestDiscordClient restClient, DiscordSharderConfiguration configuration = null)
            : base(restClient, configuration)
        {
            if (!IsBot)
                throw new ArgumentException("Only bots support sharding.", nameof(restClient));

            Shards = ReadOnlyList<Shard>.Empty;
            _shardCount = configuration.ShardCount.GetValueOrDefault();
            _getGateway = (_, guildId) =>
            {
                var client = _ as DiscordSharder;
                return client._gateways[client.GetShardId(guildId)];
            };
        }

        public override ValueTask DisposeAsync()
        {
            if (IsDisposed)
                return default;

            IsDisposed = true;

            for (var i = 0; i < _gateways.Length; i++)
                _gateways[i].Dispose();

            return base.DisposeAsync();
        }
    }
}
