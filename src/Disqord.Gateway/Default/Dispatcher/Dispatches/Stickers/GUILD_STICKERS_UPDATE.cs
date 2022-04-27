using System.Collections.Generic;
using System.Threading.Tasks;
using Qommon.Collections;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildStickersUpdateHandler : Handler<GuildStickersUpdateJsonModel, StickersUpdatedEventArgs>
    {
        public override ValueTask<StickersUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildStickersUpdateJsonModel model)
        {
            IReadOnlyDictionary<Snowflake, IGuildSticker> oldStickers;
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
                    (x, _) => x.Id,
                    (x, client) => new TransientGuildSticker(client, x) as IGuildSticker);
            }
            var e = new StickersUpdatedEventArgs(model.GuildId, guild, oldStickers, newStickers);
            return new(e);
        }
    }
}
