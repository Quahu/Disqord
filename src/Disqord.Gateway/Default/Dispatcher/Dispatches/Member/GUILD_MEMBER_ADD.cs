using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class GuildMemberAddHandler : Handler<GuildMemberAddJsonModel, MemberJoinedEventArgs>
    {
        public override ValueTask<MemberJoinedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, GuildMemberAddJsonModel model)
        {
            var guild = Client.GetGuild(model.GuildId);
            guild?.Update(model); // Increments the member count.

            var member = Dispatcher.GetOrAddMemberTransient(model.GuildId, model);
            var e = new MemberJoinedEventArgs(guild, member);
            return new(e);
        }
    }
}
