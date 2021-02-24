using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageReactionRemoveHandler : Handler<MessageReactionRemoveJsonModel, ReactionRemovedEventArgs>
    {
        public override async Task<ReactionRemovedEventArgs> HandleDispatchAsync(MessageReactionRemoveJsonModel model)
        {
            CachedUserMessage message;
            if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache))
            {
                message = messageCache.GetValueOrDefault(model.MessageId);
                message?.Update(model);
            }
            else
            {
                message = null;
            }

            return new ReactionRemovedEventArgs(model.UserId, model.ChannelId, model.MessageId, message, message.GuildId, Emoji.Create(model.Emoji));
        }
    }
}
