using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildMemberUpdateHandler : Handler<GuildMemberUpdateJsonModel, MemberUpdatedEventArgs>
    {
        public override ValueTask<MemberUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildMemberUpdateJsonModel model)
        {
            CachedMember oldMember;
            IMember newMember;
            if (CacheProvider.TryGetMembers(model.GuildId, out var cache) && cache.TryGetValue(model.User.Value.Id, out var member))
            {
                newMember = member;
                var oldUser = member.SharedUser.Clone() as CachedSharedUser;
                oldMember = member.Clone() as CachedMember;
                oldMember.SharedUser = oldUser;
                newMember.Update(model);
            }
            else
            {
                oldMember = null;
                newMember = new TransientMember(Client, model.GuildId, model);
            }

            var e = new MemberUpdatedEventArgs(oldMember, newMember);
            return new(e);
        }
    }
}
