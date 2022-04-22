using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class InviteCreateHandler : Handler<InviteCreateJsonModel, InviteCreatedEventArgs>
    {
        public override ValueTask<InviteCreatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, InviteCreateJsonModel model)
        {
            var inviter = Optional.ConvertOrDefault(model.Inviter, x => new TransientUser(Client, x)) as IUser;
            var targetUser = Optional.ConvertOrDefault(model.TargetUser, x => new TransientUser(Client, x)) as IUser;
            var targetApplication = Optional.ConvertOrDefault(model.TargetApplication, x => new TransientApplication(Client, x)) as IApplication;
            var e = new InviteCreatedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, model.Code, model.CreatedAt,
                inviter, model.MaxAge, model.MaxUses, model.TargetType.GetValueOrNullable(), targetUser, targetApplication, model.Temporary, model.Uses);
            return new(e);
        }
    }
}
