using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Serialization.Json;
using Microsoft.Extensions.Logging;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildDeleteDispatchHandler : DispatchHandler<UnavailableGuildJsonModel, EventArgs>
{
    private ReadyDispatchHandler _readyDispatchHandler = null!;

    public override void Bind(DefaultGatewayDispatcher value)
    {
        _readyDispatchHandler = (value[GatewayDispatchNames.Ready] as ReadyDispatchHandler)!;

        base.Bind(value);
    }

    public override async ValueTask HandleDispatchAsync(IShard shard, IJsonNode data)
    {
        try
        {
            await base.HandleDispatchAsync(shard, data).ConfigureAwait(false);
        }
        catch (Exception)
        {
            // If an exception occurs during deserialization, make sure to still pop the pending guild.
            if (data is IJsonObject jsonObject && jsonObject.TryGetValue("id", out var guildIdNode) && guildIdNode != null)
            {
                var guildId = guildIdNode.ToType<Snowflake>();
                _readyDispatchHandler.PopPendingGuild(shard.Id, guildId);
            }

            throw;
        }
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
