using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class StageDeleteDispatchHandler : DispatchHandler<StageInstanceJsonModel, StageDeletedEventArgs>
{
    public override ValueTask<StageDeletedEventArgs?> HandleDispatchAsync(IShard shard, StageInstanceJsonModel model)
    {
        IStage stage;
        if (CacheProvider.TryGetStages(model.GuildId, out var cache) && cache.TryRemove(model.Id, out var cachedStage))
        {
            stage = cachedStage;
        }
        else
        {
            stage = new TransientStage(Client, model);
        }

        var e = new StageDeletedEventArgs(stage);
        return new(e);
    }
}
