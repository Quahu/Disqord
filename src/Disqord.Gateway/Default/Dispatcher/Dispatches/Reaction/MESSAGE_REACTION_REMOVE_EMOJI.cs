using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageReactionRemoveEmojiHandler : Handler<MessageReactionRemoveEmojiJsonModel, ReactionsClearedEventArgs>
    {
        public override async Task<ReactionsClearedEventArgs> HandleDispatchAsync(MessageReactionRemoveEmojiJsonModel model)
        {
            CachedUserMessage message;
            Optional<IReadOnlyDictionary<IEmoji, IReaction>> oldReactions;
            if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache))
            {
                message = messageCache.GetValueOrDefault(model.MessageId);
                oldReactions = message.Reactions;
                message?.Update(model);
            }
            else
            {
                message = null;
                oldReactions = default;
            }

            return new ReactionsClearedEventArgs(model.ChannelId, model.MessageId, message, message.GuildId, Emoji.Create(model.Emoji), oldReactions);
        }
    }
}
