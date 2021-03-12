using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class VoiceStateUpdateHandler : Handler<VoiceStateJsonModel, VoiceStateUpdatedEventArgs>
    {
        public override ValueTask<VoiceStateUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, VoiceStateJsonModel model)
        {
            if (model.GuildId.HasValue)
                return new(result: null);

            var voiceState = new TransientVoiceState(Client, model);
            var e = new VoiceStateUpdatedEventArgs(voiceState);
            return new(e);
        }
    }
}
