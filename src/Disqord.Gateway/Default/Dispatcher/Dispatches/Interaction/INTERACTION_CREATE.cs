using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway.Default.Dispatcher;

public class InteractionCreateDispatchHandler : DispatchHandler<InteractionJsonModel, InteractionReceivedEventArgs>
{
    public override ValueTask<InteractionReceivedEventArgs?> HandleDispatchAsync(IShard shard, InteractionJsonModel model)
    {
        var now = Stopwatch.GetTimestamp();
        CachedMember? member = null;
        var interaction = TransientUserInteraction.Create(Client, now, model);
        if (model.GuildId.HasValue
            && Client.CacheProvider.TryGetUsers(out var userCache)
            && Client.CacheProvider.TryGetMembers(model.GuildId.Value, out var memberCache))
        {
            member = Dispatcher.GetOrAddMember(userCache, memberCache, model.GuildId.Value, model.Member.Value);

            if (interaction is IApplicationCommandInteraction applicationCommandInteraction
                && applicationCommandInteraction.Entities is TransientApplicationCommandInteractionEntities transientApplicationCommandInteractionEntities
                && model.Data.TryGetValue(out var dataModel) && dataModel.Resolved.TryGetValue(out var dataResolvedModel)
                && dataResolvedModel.Users.TryGetValue(out var userModels) && dataResolvedModel.Members.TryGetValue(out var memberModels))
            {
                var users = new Dictionary<Snowflake, IUser>(memberModels.Count);
                foreach (var (id, memberModel) in memberModels)
                {
                    if (!userModels.TryGetValue(id, out var userModel))
                        continue;

                    memberModel.User = userModel;
                    users.Add(id, Dispatcher.GetOrAddMemberTransient(model.GuildId.Value, memberModel));
                }

                transientApplicationCommandInteractionEntities._users = users.ReadOnly();
            }
        }

        var e = new InteractionReceivedEventArgs(interaction, member);
        return new(e);
    }
}
