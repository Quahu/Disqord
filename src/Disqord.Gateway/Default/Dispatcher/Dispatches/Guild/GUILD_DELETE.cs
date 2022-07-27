using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Microsoft.Extensions.Logging;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildDeleteDispatchHandler : DispatchHandler<UnavailableGuildJsonModel, EventArgs>
{
    private ReadyDispatchHandler _readyDispatchHandler = null!;

    public override void Bind(DefaultGatewayDispatcher value)
    {
        _readyDispatchHandler = (value[GatewayDispatchNames.Ready] as ReadyDispatchHandler)!;

        base.Bind(value);
    }

    public override async ValueTask<EventArgs?> HandleDispatchAsync(IShard shard, UnavailableGuildJsonModel model)
    {
        CachedGuild? guild = null;
        var isPending = _readyDispatchHandler.IsPendingGuild(shard.Id, model.Id);
        if (model.Unavailable.HasValue || isPending) // Note: apparently `model.Unavailable` is provided for pending GUILD_CREATEs but not GUILD_DELETEs.
        {
            try
            {
                if (CacheProvider.TryGetGuilds(out var cache))
                {
                    guild = cache.GetValueOrDefault(model.Id);
                    guild?.Update(model);
                }

                if (isPending)
                {
                    shard.Logger.LogInformation("Pending guild {0} is unavailable.", model.Id.RawValue);
                }
                else
                {
                    if (guild != null)
                        shard.Logger.LogInformation("Guild {0} ({1}) became unavailable.", guild.Name, guild.Id.RawValue);
                    else
                        shard.Logger.LogInformation("Uncached guild {0} became unavailable.", model.Id.RawValue);
                }

                //  Invoke the event and possibly invoke ready afterwards.
                await InvokeEventAsync(new GuildUnavailableEventArgs(model.Id, guild)).ConfigureAwait(false);
            }
            finally
            {
                if (isPending)
                    _readyDispatchHandler.PopPendingGuild(shard.Id, model.Id);
            }

            return null;
        }

        CacheProvider.Reset(model.Id, out guild);
        if (guild != null)
            shard.Logger.LogInformation("Left guild '{0}' ({1}).", guild.Name, guild.Id.RawValue);
        else
            shard.Logger.LogInformation("Left uncached guild {0}.", model.Id.RawValue);

        return new LeftGuildEventArgs(model.Id, guild);
    }
}
