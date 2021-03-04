using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageCreateHandler : Handler<MessageJsonModel, MessageReceivedEventArgs>
    {
        public override async Task<MessageReceivedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, MessageJsonModel model)
        {
            CachedMember author = null;
            IGatewayMessage message = null;
            if (model.GuildId.HasValue && IsUserMessage(model))
            {
                model.Member.Value.User = model.Author;
                author = Dispatcher.GetOrAddMember(model.GuildId.Value, model.Member.Value);

                if (CacheProvider.TryGetMessages(model.ChannelId, out var messageCache))
                {
                    message = new CachedUserMessage(Client, author, model);
                    messageCache.Add(model.Id, message as CachedUserMessage);
                }
            }

            if (message == null)
                message = GatewayTransientMessage.Create(Client, model);

            CachedTextChannel channel = null;
            if (model.GuildId.HasValue && CacheProvider.TryGetChannels(model.ChannelId, out var channelCache))
            {
                channel = channelCache.GetValueOrDefault(model.ChannelId) as CachedTextChannel;
                if (channel != null)
                {
                    channel.LastMessageId = model.Id;
                }
            }

            return new MessageReceivedEventArgs(message, channel, author);
        }

        private static bool IsUserMessage(MessageJsonModel model)
        {
            switch ((MessageType) model.Type)
            {
                case MessageType.Default:
                case MessageType.Reply:
                case MessageType.ApplicationCommand:
                    return !model.WebhookId.HasValue;
            }

            return false;
        }
    }
}
