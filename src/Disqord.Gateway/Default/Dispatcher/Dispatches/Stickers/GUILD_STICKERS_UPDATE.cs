using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildStickersUpdateDispatchHandler : DispatchHandler<GuildStickersUpdateJsonModel, StickersUpdatedEventArgs>
{
    public override ValueTask<StickersUpdatedEventArgs?> HandleDispatchAsync(IShard shard, GuildStickersUpdateJsonModel model)
    {
        IReadOnlyDictionary<Snowflake, IGuildSticker>? oldStickers;
        IReadOnlyDictionary<Snowflake, IGuildSticker> newStickers;
        if (CacheProvider.TryGetGuilds(out var cache) && cache.TryGetValue(model.GuildId, out var guild))
        {
            oldStickers = guild.Stickers;
            guild.Update(model);
            newStickers = guild.Stickers;
        }
        else
        {
            guild = null;
            oldStickers = null;
            newStickers = model.Stickers.ToReadOnlyDictionary(Client,
                (model, _) => model.Id,
                (model, client) => new TransientGuildSticker(client, model) as IGuildSticker);
        }
        var e = new StickersUpdatedEventArgs(model.GuildId, guild, oldStickers, newStickers);
        return new(e);
    }
}
