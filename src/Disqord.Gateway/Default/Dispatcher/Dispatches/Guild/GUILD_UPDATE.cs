using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildUpdateDispatchHandler : DispatchHandler<GuildJsonModel, GuildUpdatedEventArgs>
{
    public override ValueTask<GuildUpdatedEventArgs?> HandleDispatchAsync(IShard shard, GuildJsonModel model)
    {
        CachedGuild? oldGuild;
        IGuild newGuild;
        if (CacheProvider.TryGetGuilds(out var cache) && cache.TryGetValue(model.Id, out var guild))
        {
            newGuild = guild;
            oldGuild = guild.Clone() as CachedGuild;
            newGuild.Update(model);
        }
        else
        {
            oldGuild = null;
            newGuild = new TransientGuild(Client, model);
        }

        var e = new GuildUpdatedEventArgs(oldGuild, newGuild);
        return new(e);
    }
}
