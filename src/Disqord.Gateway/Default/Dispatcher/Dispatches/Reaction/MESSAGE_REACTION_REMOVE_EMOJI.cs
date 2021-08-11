using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageReactionRemoveEmojiHandler : Handler<MessageReactionRemoveEmojiJsonModel, ReactionsClearedEventArgs>
    {
        public override ValueTask<ReactionsClearedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, MessageReactionRemoveEmojiJsonModel model)
        {
            CachedUserMessage message;
            Optional<IReadOnlyDictionary<IEmoji, MessageReaction>> oldReactions;
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

            var e = new ReactionsClearedEventArgs(model.GuildId.GetValueOrNullable(), model.ChannelId, model.MessageId, message, Emoji.Create(model.Emoji), oldReactions);
            return new(e);
        }
    }
}
