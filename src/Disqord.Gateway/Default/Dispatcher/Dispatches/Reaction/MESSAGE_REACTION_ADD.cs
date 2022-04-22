using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;
using Qommon.Collections.Synchronized;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageReactionAddHandler : Handler<MessageReactionAddJsonModel, ReactionAddedEventArgs>
    {
        public override ValueTask<ReactionAddedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, MessageReactionAddJsonModel model)
        {
            CachedUserMessage message;
            IMember member = null;
            if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache))
            {
                message = messageCache.GetValueOrDefault(model.MessageId);
                message?.Update(model);
            }
            else
            {
                message = null;
            }

            if (model.GuildId.HasValue)
                member = Dispatcher.GetOrAddMemberTransient(model.GuildId.Value, model.Member.Value);

            var e = new ReactionAddedEventArgs(model.GuildId.GetValueOrNullable(), model.UserId, model.ChannelId, model.MessageId, message, member, TransientEmoji.Create(model.Emoji));
            return new(e);
        }
    }
}
