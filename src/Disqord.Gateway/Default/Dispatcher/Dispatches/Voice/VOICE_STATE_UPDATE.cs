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

            CachedVoiceState oldVoiceState = null;
            IVoiceState newVoiceState = null;
            if (CacheProvider.TryGetVoiceStates(model.GuildId.Value, out var cache))
            {
                if (model.ChannelId != null)
                {
                    if (cache.TryGetValue(model.UserId, out var voiceState))
                    {
                        newVoiceState = voiceState;
                        oldVoiceState = voiceState.Clone() as CachedVoiceState;
                        newVoiceState.Update(model);
                    }
                    else
                    {
                        newVoiceState = new CachedVoiceState(Client, model.GuildId.Value, model);
                        cache.Add(model.UserId, newVoiceState as CachedVoiceState);
                    }
                }
                else
                {
                    cache.TryRemove(model.UserId, out oldVoiceState);
                }
            }

            newVoiceState ??= new TransientVoiceState(Client, model);

            IMember member = Dispatcher.GetOrAddMember(model.GuildId.Value, model.Member.Value);
            if (member == null)
                member = new TransientMember(Client, model.GuildId.Value, model.Member.Value);

            var e = new VoiceStateUpdatedEventArgs(member, oldVoiceState, newVoiceState);
            return new(e);
        }
    }
}
