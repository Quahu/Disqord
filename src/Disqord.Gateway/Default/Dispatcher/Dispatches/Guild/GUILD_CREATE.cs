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
                if (CacheProvider.TryGetGuilds(out var cache))
                {
                    if (!isInitial)
                    {
                        guild = cache.GetValueOrDefault(model.Id);
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
                        cache.Add(model.Id, guild as CachedGuild);
                    }
                }
                else
                {
                    guild = new GatewayTransientGuild(Client, model);
                }

                var logLevel = isInitial
                    ? LogLevel.Debug
                    : LogLevel.Information;
                Logger.Log(logLevel, "Guild {0} ({1}) became available.", guild.Name, guild.Id.RawValue);
                return new GuildAvailableEventArgs(guild);
            }
            else
            {
                // We joined a new guild.
                if (Client.CacheProvider.TryGetCache<CachedGuild>(out var cache))
                {
                    guild = new CachedGuild(Client, model);
                    cache.Add(model.Id, guild as CachedGuild);
                }
                else
                {
                    guild = new GatewayTransientGuild(Client, model);
                }

                Logger.LogInformation("Joined guild {0} ({1}).", guild.Name, guild.Id.RawValue);
                return new JoinedGuildEventArgs(guild);
            }
        }
    }
}
