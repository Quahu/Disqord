using System.Collections.Generic;
using Disqord.Bot.Prefixes;
using Disqord.Rest;
using Disqord.Sharding;

namespace Disqord.Bot.Sharding
{
    public partial class DiscordBotSharder : DiscordBotBase, IDiscordSharder
    {
        public IReadOnlyList<Shard> Shards => (_client as DiscordSharder).Shards;

        public DiscordBotSharder(TokenType tokenType, string token, IPrefixProvider prefixProvider, DiscordBotSharderConfiguration configuration = null)
            : base(new DiscordSharder(tokenType, token, configuration ??= new DiscordBotSharderConfiguration()), prefixProvider, configuration)
        { }

        public DiscordBotSharder(RestDiscordClient restClient, IPrefixProvider prefixProvider, DiscordBotSharderConfiguration configuration = null)
            : base(new DiscordSharder(restClient, configuration ??= new DiscordBotSharderConfiguration()), prefixProvider, configuration)
        { }

        public int GetShardId(Snowflake guildId)
            => (_client as DiscordSharder).GetShardId(guildId);
    }
}