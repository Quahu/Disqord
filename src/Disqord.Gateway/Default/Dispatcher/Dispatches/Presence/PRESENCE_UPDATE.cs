using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class PresenceUpdateDispatchHandler : DispatchHandler<PresenceJsonModel, PresenceUpdatedEventArgs>
{
    public override ValueTask<PresenceUpdatedEventArgs?> HandleDispatchAsync(IShard shard, PresenceJsonModel model)
    {
        if (model.GuildId == default) // just in case?
            return new(result: null);

        CachedPresence? oldPresence = null;
        IPresence? newPresence = null;
        if (CacheProvider.TryGetPresences(model.GuildId, out var cache))
        {
            if (model.Status != UserStatus.Offline)
            {
                if (cache.TryGetValue(model.User.Id, out var presence))
                {
                    newPresence = presence;
                    oldPresence = presence.Clone() as CachedPresence;
                    newPresence.Update(model);
                }
                else
                {
                    newPresence = new CachedPresence(Client, model);
                    cache.Add(model.User.Id, (newPresence as CachedPresence)!);
                }
            }
            else
            {
                cache.TryRemove(model.User.Id, out oldPresence);
            }
        }

        newPresence ??= new TransientPresence(Client, model);

        var user = Optional.Conditional(model.User.Username != null, state =>
        {
            var (client, model) = state;
            return new TransientUser(client, model) as IUser;
        }, (Client, model.User));

        var e = new PresenceUpdatedEventArgs(model.GuildId, user, oldPresence, newPresence);
        return new(e);
    }
}
