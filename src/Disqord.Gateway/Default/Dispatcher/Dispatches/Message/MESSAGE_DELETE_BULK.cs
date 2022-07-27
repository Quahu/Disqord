using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class MessageDeleteBulkDispatchHandler : DispatchHandler<MessageDeleteBulkJsonModel, MessagesDeletedEventArgs>
{
    public override ValueTask<MessagesDeletedEventArgs?> HandleDispatchAsync(IShard shard, MessageDeleteBulkJsonModel model)
    {
        if (!model.GuildId.HasValue)
            return new(result: null);

        var messages = new Dictionary<Snowflake, CachedUserMessage>();
        if (model.GuildId.HasValue && CacheProvider.TryGetMessages(model.ChannelId, out var cache))
        {
            for (var i = 0; i < model.Ids.Length; i++)
            {
                var id = model.Ids[i];
                if (cache.TryRemove(id, out var message))
                    messages.Add(id, message);
            }
        }

        var e = new MessagesDeletedEventArgs(model.GuildId.Value, model.ChannelId, model.Ids, messages);
        return new(e);
    }
}
