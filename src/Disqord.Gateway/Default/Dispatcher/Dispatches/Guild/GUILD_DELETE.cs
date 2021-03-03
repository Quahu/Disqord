using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildDeleteHandler : Handler<GatewayGuildJsonModel, EventArgs>
    {
        public override async Task<EventArgs> HandleDispatchAsync(IGatewayApiClient shard, GatewayGuildJsonModel model)
        {
            CachedGuild guild = null;
            if (Client.CacheProvider.TryGetCache<CachedGuild>(out var cache))
            {
                //guild = await cache.RemoveAsync(model.Id).ConfigureAwait(false);
            }

            if (model.Unavailable.HasValue)
            {
                if (guild == null)
                {
                    // Shouldn't happen?
                    shard.Logger.LogWarning("Guild {0} is uncached and became unavailable.", model.Id);
                    return null;
                }

                // TODO: set guild unavailable
                shard.Logger.LogInformation("Guild '{0}' ({1}) became unavailable.", guild.Name, guild.Id.RawValue);
                return new GuildUnavailableEventArgs(guild);
            }
            else
            {
                if (guild == null)
                {
                    // Shouldn't happen?
                    shard.Logger.LogWarning("Left uncached guild {0}.", model.Id);
                    return null;
                }

                //foreach (var member in guild.Members.Values)
                //    member.SharedUser.References--;

                shard.Logger.LogInformation("Left guild '{0}' ({1}).", guild.Name, guild.Id.RawValue);
                return new LeftGuildEventArgs(guild);
            }
        }
    }
}
