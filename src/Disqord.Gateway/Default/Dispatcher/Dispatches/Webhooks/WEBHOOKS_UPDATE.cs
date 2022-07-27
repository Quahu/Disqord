using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class WebhooksUpdateDispatchHandler : DispatchHandler<WebhooksUpdateJsonModel, WebhooksUpdatedEventArgs>
{
    public override ValueTask<WebhooksUpdatedEventArgs?> HandleDispatchAsync(IShard shard, WebhooksUpdateJsonModel model)
    {
        var e = new WebhooksUpdatedEventArgs(model.GuildId, model.ChannelId);
        return new(e);
    }
}
