using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class MessageReactionRemoveAllDispatchHandler : DispatchHandler<MessageReactionRemoveAllJsonModel, ReactionsClearedEventArgs>
{
    public override ValueTask<ReactionsClearedEventArgs?> HandleDispatchAsync(IShard shard, MessageReactionRemoveAllJsonModel model)
    {
        CachedUserMessage? message;
        Optional<IReadOnlyDictionary<IEmoji, IMessageReaction>> oldReactions;
        if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache))
        {
            message = messageCache.GetValueOrDefault(model.MessageId);
            oldReactions = message?.Reactions ?? default;
            message?.Update(model);
        }
        else
        {
            message = null;
            oldReactions = default;
        }

        var e = new ReactionsClearedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, model.MessageId, message, null, oldReactions);
        return new(e);
    }
}
