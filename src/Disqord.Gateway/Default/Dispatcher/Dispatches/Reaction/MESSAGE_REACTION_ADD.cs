using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

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
                member = Dispatcher.GetOrAddMember(model.GuildId.Value, model.Member.Value) ?? new TransientMember(Client, model.GuildId.Value, model.Member.Value) as IMember;

            var e = new ReactionAddedEventArgs(model.GuildId.GetValueOrNullable(), model.UserId, model.ChannelId, model.MessageId, message, member, Emoji.Create(model.Emoji));
            return new(e);
        }
    }
}
