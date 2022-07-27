using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;
using Qommon.Collections.ReadOnly;

namespace Disqord.Gateway.Default.Dispatcher;

public class ThreadMembersUpdateDispatchHandler : DispatchHandler<ThreadMembersUpdateJsonModel, ThreadMembersUpdatedEventArgs>
{
    public override ValueTask<ThreadMembersUpdatedEventArgs?> HandleDispatchAsync(IShard shard, ThreadMembersUpdateJsonModel model)
    {
        var thread = Client.GetChannel(model.GuildId, model.Id) as CachedThreadChannel;
        thread?.Update(model);
        var addedMembers = new Dictionary<Snowflake, IThreadMember>();
        if (model.AddedMembers.HasValue)
        {
            foreach (var memberModel in model.AddedMembers.Value)
                addedMembers.Add(memberModel.UserId.Value, new TransientThreadMember(Client, memberModel));
        }
        var removedMemberIds = Optional.ConvertOrDefault(model.RemovedMemberIds, x => x.ReadOnly(), ReadOnlyList<Snowflake>.Empty);
        var e = new ThreadMembersUpdatedEventArgs(model.GuildId, model.Id, thread, model.MemberCount, addedMembers, removedMemberIds);
        return new(e);
    }
}
