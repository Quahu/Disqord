using System.Collections.Generic;
using System.Threading.Tasks;
using Qommon.Collections;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildEmojisUpdateHandler : Handler<GuildEmojisUpdateJsonModel, EmojisUpdatedEventArgs>
    {
        public override ValueTask<EmojisUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildEmojisUpdateJsonModel model)
        {
            IReadOnlyDictionary<Snowflake, IGuildEmoji> oldEmojis;
            IReadOnlyDictionary<Snowflake, IGuildEmoji> newEmojis;
            if (CacheProvider.TryGetGuilds(out var cache) && cache.TryGetValue(model.GuildId, out var guild))
            {
                oldEmojis = guild.Emojis;
                guild.Update(model);
                newEmojis = guild.Emojis;
            }
            else
            {
                guild = null;
                oldEmojis = null;
                newEmojis = model.Emojis.ToReadOnlyDictionary((Client, model.GuildId), (x, _) => x.Id.Value, (x, tuple) =>
                {
                    var (client, guildId) = tuple;
                    return new TransientGuildEmoji(client, guildId, x) as IGuildEmoji;
                });
            }
            var e = new EmojisUpdatedEventArgs(model.GuildId, guild, oldEmojis, newEmojis);
            return new(e);
        }
    }
}
