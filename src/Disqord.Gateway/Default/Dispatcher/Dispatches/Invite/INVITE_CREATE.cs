using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class InviteCreateHandler : Handler<InviteCreateJsonModel, InviteCreatedEventArgs>
    {
        public override ValueTask<InviteCreatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, InviteCreateJsonModel model)
        {
            var invite = new TransientInvite(Client, new InviteJsonModel
            {
                Code = model.Code,
                Inviter = model.Inviter,
                MaxUses = model.MaxUses,
                MaxAge = model.MaxAge,
                Temporary = model.Temporary,
                CreatedAt = model.CreatedAt,
                Channel = new ChannelJsonModel
                {
                    Id = model.ChannelId
                }
            });
            var e = new InviteCreatedEventArgs(model.GuildId, invite);
            return new(e);
        }
    }
}
