using System.Threading.Tasks;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageUpdateHandler : Handler<MessageUpdateJsonModel, MessageUpdatedEventArgs>
    {
        public override async Task<MessageUpdatedEventArgs> HandleDispatchAsync(MessageUpdateJsonModel model)
        {
            CachedUserMessage oldMessage;
            CachedUserMessage message;
            if (CacheProvider.TryGetMessages(model.ChannelId, out var cache) && cache.TryGetValue(model.Id, out message))
            {
                oldMessage = (CachedUserMessage) message.Clone();
                message.Update(model);
            }
            else
            {
                oldMessage = null;
                message = null;
            }

            return new MessageUpdatedEventArgs(oldMessage, message, model);
        }
    }
}
