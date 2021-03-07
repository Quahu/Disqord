using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildCreateHandler : Handler<GatewayGuildJsonModel, EventArgs>
    {
        private ReadyHandler _readyHandler;

        public override void Bind(DefaultGatewayDispatcher value)
        {
            _readyHandler = value["READY"] as ReadyHandler;

            base.Bind(value);
        }

        public override async Task<EventArgs> HandleDispatchAsync(IGatewayApiClient shard, GatewayGuildJsonModel model)
        {
            IGatewayGuild guild = null;
            // Check if the event is guild availability or we joined a new guild.
            if (model.Unavailable.HasValue)
            {
                // A guild became available.
                var isPending = _readyHandler.IsPendingGuild(shard.Id, model.Id);
                if (CacheProvider.TryGetGuilds(out var cache))
                {
                    if (isPending)
                    {
                        guild = new CachedGuild(Client, model);
                        cache.Add(model.Id, guild as CachedGuild);
                    }
                    else
                    {
                        guild = cache.GetValueOrDefault(model.Id);
                        guild?.Update(model);
                    }
                }

                if (guild == null)
                    guild = new GatewayTransientGuild(Client, model);

                var logLevel = isPending
                    ? LogLevel.Debug
                    : LogLevel.Information;
                var message = isPending
                    ? "Pending guild {0} ({1}) is available."
                    : "Guild {0} ({1}) became available.";
                shard.Logger.Log(logLevel, message, guild.Name, guild.Id.RawValue);

                //  Invoke the event and possibly invoke ready afterwards.
                await InvokeEventAsync(new GuildAvailableEventArgs(guild)).ConfigureAwait(false);

                if (isPending)
                    _readyHandler.PopPendingGuild(shard.Id, model.Id);

                return null;
            }
            else
            {
                // We joined a new guild.
                if (Client.CacheProvider.TryGetGuilds(out var cache))
                {
                    guild = new CachedGuild(Client, model);
                    cache.Add(model.Id, guild as CachedGuild);
                }
                else
                {
                    guild = new GatewayTransientGuild(Client, model);
                }

                shard.Logger.LogInformation("Joined guild {0} ({1}).", guild.Name, guild.Id.RawValue);
                return new JoinedGuildEventArgs(guild);
            }
        }
    }
}
