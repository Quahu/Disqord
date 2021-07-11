using System.Linq;
using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageCreateHandler : Handler<MessageJsonModel, MessageReceivedEventArgs>
    {
        public override ValueTask<MessageReceivedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, MessageJsonModel model)
        {
            CachedMember author = null;
            IGatewayMessage message = null;
            if (model.GuildId.HasValue && !model.WebhookId.HasValue && model.Member.HasValue
                && Client.CacheProvider.TryGetUsers(out var userCache)
                && Client.CacheProvider.TryGetMembers(model.GuildId.Value, out var memberCache))
            {
                model.Member.Value.User = model.Author;
                author = Dispatcher.GetOrAddMember(userCache, memberCache, model.GuildId.Value, model.Member.Value);
                foreach (var memberModel in model.Mentions.Select(static x =>
                {
                    var memberModel = x["member"].ToType<MemberJsonModel>();
                    memberModel.User = x;
                    return memberModel;
                }))
                {
                    Dispatcher.GetOrAddMember(userCache, memberCache, model.GuildId.Value, memberModel);
                }

                if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache)
                    && model.Type is MessageType.Default or MessageType.Reply or MessageType.ApplicationCommand)
                {
                    message = new CachedUserMessage(Client, author, model);
                    messageCache.Add(model.Id, message as CachedUserMessage);
                }
            }

            message ??= TransientGatewayMessage.Create(Client, model);

            CachedMessageGuildChannel channel = null;
            if (model.GuildId.HasValue && CacheProvider.TryGetChannels(model.GuildId.Value, out var channelCache))
            {
                channel = channelCache.GetValueOrDefault(model.ChannelId) as CachedMessageGuildChannel;
                if (channel != null)
                    channel.LastMessageId = model.Id;
            }

            var e = new MessageReceivedEventArgs(message, channel, author);
            return new(e);
        }
    }
}
