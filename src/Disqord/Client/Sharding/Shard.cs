using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Sharding
{
    public sealed class Shard
    {
        public TimeSpan? Latency => _gateway.Latency;

        public int Id { get; }

        public IReadOnlyDictionary<Snowflake, CachedGuild> Guilds { get; }

        private readonly DiscordSharder _sharder;
        private readonly DiscordClientGateway _gateway;

        internal Shard(DiscordSharder sharder, DiscordClientGateway gateway)
        {
            _sharder = sharder;
            _gateway = gateway;
            Id = gateway.ShardId.Value;
            Guilds = new ReadOnlyValuePredicateArgumentDictionary<Snowflake, CachedGuild, Shard>(
                _sharder.Guilds, (x, shard) => shard._sharder.GetShardId(x.Id) == shard.Id, this);
        }

        public Task SetPresenceAsync(UserStatus status)
            => _sharder.InternalSetPresenceAsync(_gateway, status);

        public Task SetPresenceAsync(LocalActivity activity)
            => _sharder.InternalSetPresenceAsync(_gateway, activity: activity);

        public Task SetPresenceAsync(UserStatus status, LocalActivity activity)
            => _sharder.InternalSetPresenceAsync(_gateway, status, activity);

    }
}
