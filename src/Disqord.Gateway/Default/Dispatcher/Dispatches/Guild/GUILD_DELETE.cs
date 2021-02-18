using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildDeleteHandler : Handler<GatewayGuildJsonModel, EventArgs>
    {
        public override async Task<EventArgs> HandleDispatchAsync(GatewayGuildJsonModel model)
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
                    Logger.LogWarning("Guild {0} is uncached and became unavailable.", model.Id);
                    return null;
                }

                // TODO: set guild unavailable
                Logger.LogInformation("Guild '{0}' ({1}) became unavailable.", guild.Name, guild.Id);
                return new GuildUnavailableEventArgs(guild);
            }
            else
            {
                if (guild == null)
                {
                    // Shouldn't happen?
                    Logger.LogWarning("Left uncached guild {0}.", model.Id);
                    return null;
                }

                //foreach (var member in guild.Members.Values)
                //    member.SharedUser.References--;

                Logger.LogInformation("Left guild '{0}' ({1}).", guild.Name, guild.Id);
                return new LeftGuildEventArgs(guild);
            }
        }
    }
}
