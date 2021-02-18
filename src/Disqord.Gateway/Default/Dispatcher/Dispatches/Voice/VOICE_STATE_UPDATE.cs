using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class VoiceStateUpdateHandler : Handler<VoiceStateJsonModel, VoiceStateUpdatedEventArgs>
    {
        public override async Task<VoiceStateUpdatedEventArgs> HandleDispatchAsync(VoiceStateJsonModel model)
        {
            if (model.GuildId.HasValue)
                return null;

            var voiceState = new TransientVoiceState(Client, model);
            return new VoiceStateUpdatedEventArgs(voiceState);
        }
    }
}
