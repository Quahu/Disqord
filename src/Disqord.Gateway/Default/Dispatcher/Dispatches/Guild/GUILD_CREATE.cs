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

        public override async ValueTask<EventArgs> HandleDispatchAsync(IGatewayApiClient shard, GatewayGuildJsonModel model)
        {
            IGatewayGuild guild = null;
            // Check if the event is guild availability or we joined a new guild.
            if (model.Unavailable.HasValue)
            {
                // A guild became available.
                var isPending = _readyHandler.IsPendingGuild(shard.Id, model.Id);
                try
                {
                    if (CacheProvider.TryGetGuilds(out var guildCache))
                    {
                        if (isPending)
                        {
                            guild = new CachedGuild(Client, model);
                            guildCache.Add(model.Id, guild as CachedGuild);
                        }
                        else
                        {
                            guild = guildCache.GetValueOrDefault(model.Id);
                            guild?.Update(model);
                        }
                    }

                    if (guild == null)
                        guild = new TransientGatewayGuild(Client, model);

                    // TODO: optimise member cache retrieval
                    if (CacheProvider.TryGetMembers(model.Id, out var memberCache))
                    {
                        foreach (var memberModel in model.Members)
                            Dispatcher.GetOrAddMember(model.Id, memberModel);
                    }
                    
                    if (CacheProvider.TryGetChannels(model.Id, out var channelCache))
                    {
                        foreach (var channelModel in model.Channels)
                        {
                            if (isPending)
                            {
                                var channel = CachedGuildChannel.Create(Client, model.Id, channelModel);
                                channelCache.Add(channel.Id, channel);
                            }
                            else
                            {
                                var channel = channelCache.GetValueOrDefault(channelModel.Id);
                                channel?.Update(channelModel);
                            }
                        }
                    }

                    if (CacheProvider.TryGetRoles(model.Id, out var roleCache))
                    {
                        foreach (var roleModel in model.Roles)
                        {
                            if (isPending)
                            {
                                var role = new CachedRole(Client, model.Id, roleModel);
                                roleCache.Add(role.Id, role);
                            }
                            else
                            {
                                var role = roleCache.GetValueOrDefault(roleModel.Id);
                                role?.Update(roleModel);
                            }
                        }
                    }

                    var logLevel = isPending
                        ? LogLevel.Debug
                        : LogLevel.Information;
                    var message = isPending
                        ? "Pending guild {0} ({1}) is available."
                        : "Guild {0} ({1}) became available.";
                    shard.Logger.Log(logLevel, message, guild.Name, guild.Id.RawValue);

                    //  Invoke the event and possibly invoke ready afterwards.
                    await InvokeEventAsync(new GuildAvailableEventArgs(guild)).ConfigureAwait(false);
                }
                finally
                {
                    if (isPending)
                        _readyHandler.PopPendingGuild(shard.Id, model.Id);
                }

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
                    guild = new TransientGatewayGuild(Client, model);
                }
                
                // TODO: optimise member cache retrieval
                if (CacheProvider.TryGetMembers(model.Id, out var memberCache))
                {
                    foreach (var memberModel in model.Members)
                        Dispatcher.GetOrAddMember(model.Id, memberModel);
                }

                if (CacheProvider.TryGetChannels(model.Id, out var channelCache))
                {
                    foreach (var channelModel in model.Channels)
                    {
                        var channel = CachedGuildChannel.Create(Client, model.Id, channelModel);
                        channelCache.Add(channel.Id, channel);
                    }
                }

                if (CacheProvider.TryGetRoles(model.Id, out var roleCache))
                {
                    foreach (var roleModel in model.Roles)
                    {
                        var role = new CachedRole(Client, model.Id, roleModel);
                        roleCache.Add(role.Id, role);
                    }
                }

                shard.Logger.LogInformation("Joined guild {0} ({1}).", guild.Name, guild.Id.RawValue);
                return new JoinedGuildEventArgs(guild);
            }
        }
    }
}
