using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class MessageUpdateHandler : Handler<MessageUpdateJsonModel, MessageUpdatedEventArgs>
    {
        public override async Task<MessageUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, MessageUpdateJsonModel model)
        {
            CachedUserMessage oldMessage;
            CachedUserMessage newMessage;
            if (CacheProvider.TryGetMessages(model.ChannelId, out var cache) && cache.TryGetValue(model.Id, out newMessage))
            {
                oldMessage = newMessage.Clone() as CachedUserMessage;
                newMessage.Update(model);
            }
            else
            {
                oldMessage = null;
                newMessage = null;
            }

            return new MessageUpdatedEventArgs(oldMessage, newMessage, model);
        }
    }
}
