using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Interaction;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class InteractionCreateHandler : Handler<InteractionJsonModel, InteractionReceivedEventArgs>
    {
        public override ValueTask<InteractionReceivedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, InteractionJsonModel model)
        {
            CachedMember member = null;
            if (model.GuildId.HasValue)
                member = Dispatcher.GetOrAddMember(model.GuildId.Value, model.Member.Value);

            var interaction = TransientInteraction.Create(Client, model);
            var e = new InteractionReceivedEventArgs(interaction, member);
            return new(e);
        }
    }
}
