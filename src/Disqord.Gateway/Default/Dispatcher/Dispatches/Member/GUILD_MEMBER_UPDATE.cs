using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildMemberUpdateHandler : Handler<GuildMemberUpdateJsonModel, MemberUpdatedEventArgs>
    {
        public override ValueTask<MemberUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildMemberUpdateJsonModel model)
        {
            CachedMember oldMember = null;
            IMember newMember = null;
            if (CacheProvider.TryGetMembers(model.GuildId, out var memberCache))
            {
                if (memberCache.TryGetValue(model.User.Value.Id, out var member))
                {
                    newMember = member;
                    var oldUser = member.SharedUser.Clone() as CachedSharedUser;
                    oldMember = member.Clone() as CachedMember;
                    oldMember.SharedUser = oldUser;
                    newMember.Update(model);
                }
                else if (CacheProvider.TryGetUsers(out var userCache))
                {
                    newMember = Dispatcher.GetOrAddMember(userCache, memberCache, model.GuildId, model);
                }
            }

            newMember ??= new TransientMember(Client, model.GuildId, model);

            var e = new MemberUpdatedEventArgs(oldMember, newMember);
            return new(e);
        }
    }
}
