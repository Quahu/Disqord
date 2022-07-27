using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class MessageUpdateDispatchHandler : DispatchHandler<MessageUpdateJsonModel, MessageUpdatedEventArgs>
{
    public override ValueTask<MessageUpdatedEventArgs?> HandleDispatchAsync(IShard shard, MessageUpdateJsonModel model)
    {
        CachedUserMessage? oldMessage;
        if (CacheProvider.TryGetMessages(model.ChannelId, out var cache) && cache.TryGetValue(model.Id, out var newMessage))
        {
            oldMessage = newMessage.Clone() as CachedUserMessage;
            newMessage.Update(model);
        }
        else
        {
            oldMessage = null;
            newMessage = null;
        }

        var e = new MessageUpdatedEventArgs(oldMessage, newMessage, model);
        return new(e);
    }
}
