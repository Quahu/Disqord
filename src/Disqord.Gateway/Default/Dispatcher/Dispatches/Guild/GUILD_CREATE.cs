using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildCreateHandler : Handler<GatewayGuildJsonModel, EventArgs>
    {
        public override async Task<EventArgs> HandleDispatchAsync(GatewayGuildJsonModel model)
        {
            IGatewayGuild guild;
            // Check if the event is guild availability or we joined a new guild.
            if (model.Unavailable.HasValue)
            {
                // A guild became (un)available.
                // TODO: !!!
                var isInitial = /*Dispatcher._initialUnavailableGuilds.Remove(model.Id);*/
                    true;
                if (Client.CacheProvider.TryGetCache<CachedGuild>(out var cache))
                {
                    if (!isInitial)
                    {
                        guild = await cache.GetAsync(model.Id).ConfigureAwait(false);
                        if (guild != null)
                        {
                            guild.Update(model);
                        }
                        else
                        {
                            guild = new GatewayTransientGuild(Client, model);
                        }
                    }
                    else
                    {
                        guild = new CachedGuild(Client, model);
                        await cache.AddAsync(guild as CachedGuild).ConfigureAwait(false);
                    }
                }
                else
                {
                    guild = new GatewayTransientGuild(Client, model);
                }

                var logLevel = isInitial
                    ? LogLevel.Debug
                    : LogLevel.Information;
                Logger.Log(logLevel, "Guild {0} ({1}) became available.", guild.Name, guild.Id);
                return new GuildAvailableEventArgs(guild);
            }
            else
            {
                // We joined a new guild.
                if (Client.CacheProvider.TryGetCache<CachedGuild>(out var cache))
                {
                    guild = new CachedGuild(Client, model);
                    await cache.AddAsync(guild as CachedGuild).ConfigureAwait(false);
                }
                else
                {
                    guild = new GatewayTransientGuild(Client, model);
                }

                Logger.LogInformation("Joined guild {0} ({1}).", guild.Name, guild.Id);
                return new JoinedGuildEventArgs(guild);
            }
        }
    }
}
