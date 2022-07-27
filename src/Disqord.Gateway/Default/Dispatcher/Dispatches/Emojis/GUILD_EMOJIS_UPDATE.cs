using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildEmojisUpdateDispatchHandler : DispatchHandler<GuildEmojisUpdateJsonModel, EmojisUpdatedEventArgs>
{
    public override ValueTask<EmojisUpdatedEventArgs?> HandleDispatchAsync(IShard shard, GuildEmojisUpdateJsonModel model)
    {
        IReadOnlyDictionary<Snowflake, IGuildEmoji>? oldEmojis;
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
            newEmojis = model.Emojis.ToReadOnlyDictionary((Client, model.GuildId), (model, _) => model.Id!.Value, (model, state) =>
            {
                var (client, guildId) = state;
                return new TransientGuildEmoji(client, guildId, model) as IGuildEmoji;
            });
        }

        var e = new EmojisUpdatedEventArgs(model.GuildId, guild, oldEmojis, newEmojis);
        return new(e);
    }
}
