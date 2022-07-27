using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher;

public class MessageReactionRemoveDispatchHandler : DispatchHandler<MessageReactionRemoveJsonModel, ReactionRemovedEventArgs>
{
    public override ValueTask<ReactionRemovedEventArgs?> HandleDispatchAsync(IShard shard, MessageReactionRemoveJsonModel model)
    {
        CachedUserMessage? message;
        if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache))
        {
            message = messageCache.GetValueOrDefault(model.MessageId);
            message?.Update(model);
        }
        else
        {
            message = null;
        }

        var e = new ReactionRemovedEventArgs(model.GuildId.GetValueOrNullable(), model.UserId, model.ChannelId, model.MessageId, message, TransientEmoji.Create(model.Emoji));
        return new(e);
    }
}
