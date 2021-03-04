using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageReactionAddHandler : Handler<MessageReactionAddJsonModel, ReactionAddedEventArgs>
    {
        public override async Task<ReactionAddedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, MessageReactionAddJsonModel model)
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
            {
                member = Dispatcher.GetOrAddMember(model.GuildId.Value, model.Member.Value);
                if (member == null)
                    member = new TransientMember(Client, model.GuildId.Value, model.Member.Value);
            }

            return new ReactionAddedEventArgs(model.UserId, model.ChannelId, model.MessageId, message, model.GuildId.GetValueOrNullable(), member, Emoji.Create(model.Emoji));
        }
    }
}
