using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Microsoft.Extensions.Logging;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildCreateDispatchHandler : DispatchHandler<GatewayGuildJsonModel, EventArgs>
{
    private ReadyDispatchHandler _readyDispatchHandler = null!;

    public override void Bind(DefaultGatewayDispatcher value)
    {
        _readyDispatchHandler = (value[GatewayDispatchNames.Ready] as ReadyDispatchHandler)!;

        base.Bind(value);
    }

    public override async ValueTask<EventArgs?> HandleDispatchAsync(IShard shard, GatewayGuildJsonModel model)
    {
        IGatewayGuild guild;
        var isPending = _readyDispatchHandler.IsPendingGuild(shard.Id, model.Id);
        if (model.Unavailable.HasValue || isPending) // Note: apparently `model.Unavailable` is provided for pending GUILD_CREATEs but not GUILD_DELETEs.
        {
            try
            {
                guild = UpdateCache(model, isPending);
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
                    _readyDispatchHandler.PopPendingGuild(shard.Id, model.Id);
            }

            return null;
        }

        guild = UpdateCache(model, true);
        shard.Logger.LogInformation("Joined guild {0} ({1}).", guild.Name, guild.Id.RawValue);
        return new JoinedGuildEventArgs(guild);
    }

    // TODO: possible cache inconsistencies on unavailable guilds? No idea what Discord sends nor does while it's unavailable.
    private IGatewayGuild UpdateCache(GatewayGuildJsonModel model, bool isPending)
    {
        IGatewayGuild? guild = null;
        if (CacheProvider.TryGetGuilds(out var guildCache))
        {
            if (isPending)
            {
                guild = new CachedGuild(Client, model);
                guildCache.Add(model.Id, (guild as CachedGuild)!);
            }
            else
            {
                guild = guildCache.GetValueOrDefault(model.Id);
                guild?.Update(model);
            }
        }

        guild ??= new TransientGatewayGuild(Client, model);

        if (CacheProvider.TryGetUsers(out var userCache) && CacheProvider.TryGetMembers(model.Id, out var memberCache))
        {
            foreach (var memberModel in model.Members)
                Dispatcher.GetOrAddMember(userCache, memberCache, model.Id, memberModel);
        }

        if (CacheProvider.TryGetChannels(model.Id, out var channelCache))
        {
            foreach (var channelModel in model.Channels)
            {
                if (isPending)
                {
                    channelModel.GuildId = model.Id;
                    var channel = CachedGuildChannel.Create(Client, channelModel);
                    channelCache.Add(channel.Id, channel);
                }
                else
                {
                    var channel = channelCache.GetValueOrDefault(channelModel.Id);
                    channel?.Update(channelModel);
                }
            }

            foreach (var threadModel in model.Threads)
            {
                if (isPending)
                {
                    threadModel.GuildId = model.Id;
                    if (threadModel.Member.HasValue)
                    {
                        threadModel.Member.Value.Id = threadModel.Id;
                        threadModel.Member.Value.UserId = Client.CurrentUser!.Id;
                    }

                    var channel = new CachedThreadChannel(Client, threadModel);
                    channelCache.Add(channel.Id, channel);
                }
                else
                {
                    var channel = channelCache.GetValueOrDefault(threadModel.Id);
                    channel?.Update(threadModel);
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

        if (CacheProvider.TryGetVoiceStates(model.Id, out var voiceStateCache))
        {
            foreach (var voiceStateModel in model.VoiceStates)
            {
                if (isPending)
                {
                    var voiceState = new CachedVoiceState(Client, model.Id, voiceStateModel);
                    voiceStateCache.Add(voiceState.MemberId, voiceState);
                }
                else
                {
                    var voiceState = voiceStateCache.GetValueOrDefault(voiceStateModel.UserId);
                    voiceState?.Update(voiceStateModel);
                }
            }
        }

        if (CacheProvider.TryGetPresences(model.Id, out var presenceCache))
        {
            foreach (var presenceModel in model.Presences)
            {
                if (isPending)
                {
                    var presence = new CachedPresence(Client, presenceModel);
                    presenceCache.Add(presence.MemberId, presence);
                }
                else
                {
                    var presence = presenceCache.GetValueOrDefault(presenceModel.User.Id);
                    presence?.Update(presenceModel);
                }
            }
        }

        if (CacheProvider.TryGetStages(model.Id, out var stageCache))
        {
            foreach (var stageModel in model.StageInstances)
            {
                if (isPending)
                {
                    var stage = new CachedStage(Client, stageModel);
                    stageCache.Add(stage.Id, stage);
                }
                else
                {
                    var stage = stageCache.GetValueOrDefault(stageModel.Id);
                    stage?.Update(stageModel);
                }
            }
        }

        if (CacheProvider.TryGetGuildEvents(model.Id, out var guildEventsCache))
        {
            foreach (var guildEventModel in model.GuildScheduledEvents)
            {
                if (isPending)
                {
                    var guildEvent = new CachedGuildEvent(Client, guildEventModel);
                    guildEventsCache.Add(guildEvent.Id, guildEvent);
                }
                else
                {
                    var guildEvent = guildEventsCache.GetValueOrDefault(guildEventModel.Id);
                    guildEvent?.Update(guildEventModel);
                }
            }
        }

        return guild;
    }
}
