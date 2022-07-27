using System;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Gateway.Default.Dispatcher;

public class VoiceStateUpdateDispatchHandler : DispatchHandler<VoiceStateJsonModel, VoiceStateUpdatedEventArgs>
{
    public override ValueTask<VoiceStateUpdatedEventArgs?> HandleDispatchAsync(IShard shard, VoiceStateJsonModel model)
    {
        if (!model.GuildId.HasValue)
            return new(result: null);

        CachedVoiceState? oldVoiceState = null;
        IVoiceState? newVoiceState = null;
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
                    cache.Add(model.UserId, (newVoiceState as CachedVoiceState)!);
                }
            }
            else
            {
                cache.TryRemove(model.UserId, out oldVoiceState);
            }
        }

        newVoiceState ??= new TransientVoiceState(Client, model);

        var isLurker = false;
        if (model.Member.Value.TryGetValue("joined_at", out var joinedAt) && joinedAt is IJsonValue jsonValue && jsonValue.Value == null)
        {
            isLurker = true;
            jsonValue.Value = DateTimeOffset.UtcNow;
        }

        var memberModel = model.Member.Value.ToType<MemberJsonModel>()!;
        var member = Dispatcher.GetOrAddMemberTransient(model.GuildId.Value, memberModel);
        var e = new VoiceStateUpdatedEventArgs(member, isLurker, oldVoiceState, newVoiceState);
        return new(e);
    }
}
