using System.Threading.Tasks;
using Disqord.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageCreateHandler : Handler<MessageJsonModel, MessageReceivedEventArgs>
    {
        public override async Task<MessageReceivedEventArgs> HandleDispatchAsync(MessageJsonModel model)
        {
            IGatewayMessage message;
            if (model.GuildId.HasValue && IsUserMessageType(model.Type) && CacheProvider.TryGetMessages(model.ChannelId, out var messageCache))
            {
                message = new CachedUserMessage(Client, model);
                messageCache.Add(model.Id, message as CachedUserMessage);
            }
            else
            {
                message = GatewayTransientMessage.Create(Client, model);
            }

            CachedTextChannel channel = null;
            if (model.GuildId.HasValue && CacheProvider.TryGetChannels(model.ChannelId, out var channelCache))
            {
                channel = channelCache.GetValueOrDefault(model.ChannelId) as CachedTextChannel;
                if (channel != null)
                {
                    channel.LastMessageId = model.Id;
                }
            }

            return new MessageReceivedEventArgs(message, channel);
        }

        private static bool IsUserMessageType(int type)
        {
            switch ((MessageType) type)
            {
                case MessageType.Default:
                case MessageType.Reply:
                case MessageType.ApplicationCommand:
                    return true;
            }

            return false;
        }
    }
}
