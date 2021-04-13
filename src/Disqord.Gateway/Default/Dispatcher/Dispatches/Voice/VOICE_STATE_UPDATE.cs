using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class VoiceStateUpdateHandler : Handler<VoiceStateJsonModel, VoiceStateUpdatedEventArgs>
    {
        public override ValueTask<VoiceStateUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, VoiceStateJsonModel model)
        {
            if (!model.GuildId.HasValue)
                return new(result: null);

            CachedVoiceState oldVoiceState;
            IVoiceState newVoiceState;
            if (CacheProvider.TryGetVoiceStates(model.GuildId.Value, out var cache) && cache.TryGetValue(model.UserId, out var voiceState))
            {
                newVoiceState = voiceState;
                oldVoiceState = voiceState.Clone() as CachedVoiceState;
                newVoiceState.Update(model);
            }
            else
            {
                oldVoiceState = null;
                newVoiceState = new TransientVoiceState(Client, model);
            }

            IMember member = Dispatcher.GetOrAddMember(model.GuildId.Value, model.Member.Value);
            if (member == null)
                member = new TransientMember(Client, model.GuildId.Value, model.Member.Value);

            var e = new VoiceStateUpdatedEventArgs(member, oldVoiceState, newVoiceState);
            return new(e);
        }
    }
}
