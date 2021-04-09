using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageReactionRemoveAllHandler : Handler<MessageReactionRemoveAllJsonModel, ReactionsClearedEventArgs>
    {
        public override ValueTask<ReactionsClearedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, MessageReactionRemoveAllJsonModel model)
        {
            CachedUserMessage message;
            Optional<IReadOnlyDictionary<IEmoji, Reaction>> oldReactions;
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

            var e = new ReactionsClearedEventArgs(model.ChannelId, model.MessageId, message, model.GuildId.GetValueOrNullable(), null, oldReactions);
            return new(e);
        }
    }
}