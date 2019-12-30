using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Sharding
{
    public partial class DiscordSharder : DiscordClientBase
    {
        public sealed class Shard
        {
            public int ShardId { get; }

            public IReadOnlyDictionary<Snowflake, CachedGuild> Guilds { get; }

            private readonly DiscordSharder _client;
            private readonly DiscordClientGateway _gateway;

            internal Shard(DiscordSharder client, DiscordClientGateway gateway)
            {
                _client = client;
                _gateway = gateway;
                ShardId = gateway.ShardId.Value;
                Guilds = new ReadOnlyValuePredicateArgumentDictionary<Snowflake, CachedGuild, Shard>(
                    _client.Guilds, (x, shard) => shard._client.GetShardId(x.Id) == shard.ShardId, this);
            }

            public Task SetPresenceAsync(UserStatus status)
                => _client.InternalSetPresenceAsync(_gateway, status);

            public Task SetPresenceAsync(LocalActivity activity)
                => _client.InternalSetPresenceAsync(_gateway, activity: activity);

            public Task SetPresenceAsync(UserStatus status, LocalActivity activity)
                => _client.InternalSetPresenceAsync(_gateway, status, activity);

        }
    }
}
