using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class VoiceServerUpdateHandler : Handler<VoiceServerUpdateJsonModel, VoiceServerUpdatedEventArgs>
    {
        public override async Task<VoiceServerUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, VoiceServerUpdateJsonModel model)
        {
            return new VoiceServerUpdatedEventArgs(model.GuildId, model.Token, model.Endpoint);
        }
    }
}
