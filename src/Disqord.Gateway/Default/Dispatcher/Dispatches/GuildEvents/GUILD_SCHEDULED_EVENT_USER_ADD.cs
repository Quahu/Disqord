using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class GuildScheduledEventUserAddDispatchHandler : DispatchHandler<GuildScheduledEventUserAddJsonModel, GuildEventMemberAddedEventArgs>
{
    public override ValueTask<GuildEventMemberAddedEventArgs?> HandleDispatchAsync(IShard shard, GuildScheduledEventUserAddJsonModel model)
    {
        var e = new GuildEventMemberAddedEventArgs(model.GuildId, model.GuildScheduledEventId, model.UserId);
        return new(e);
    }
}
