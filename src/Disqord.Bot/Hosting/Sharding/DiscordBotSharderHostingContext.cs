using System.Collections.Generic;
using Disqord.Bot.Hosting;
using Disqord.Gateway.Api;
using Disqord.Sharding;

namespace Disqord.Bot.Sharding
{
    public class DiscordBotSharderHostingContext : DiscordBotHostingContext, IDiscordClientSharderConfiguration
    {
        /// <inheritdoc/>
        public int? ShardCount { get; set; }

        /// <inheritdoc/>
        public IEnumerable<ShardId> ShardIds { get; set; }
    }
}
