using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Sharding
{
    public partial class DiscordSharder : DiscordClientBase, IDiscordSharder
    {
        public IReadOnlyList<Shard> Shards { get; private set; }

        public DiscordSharder(TokenType tokenType, string token, DiscordClientConfiguration configuration = null)
            : this(new RestDiscordClient(tokenType, token, configuration?.Logger, configuration?.Serializer), configuration)
        { }

        public DiscordSharder(RestDiscordClient restClient, DiscordClientConfiguration configuration = null)
            : base(restClient, configuration?.MessageCache, configuration?.Logger, configuration?.Serializer)
        {
            if (!IsBot)
                throw new ArgumentException("Only bots support sharding.", nameof(restClient));

            configuration = configuration ?? DiscordClientConfiguration.Default;

            Shards = Array.Empty<Shard>();
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
