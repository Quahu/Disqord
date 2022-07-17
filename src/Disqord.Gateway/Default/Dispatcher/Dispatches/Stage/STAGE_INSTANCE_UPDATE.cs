using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class StageUpdateHandler : Handler<StageInstanceJsonModel, StageUpdatedEventArgs>
{
    public override ValueTask<StageUpdatedEventArgs?> HandleDispatchAsync(IGatewayApiClient shard, StageInstanceJsonModel model)
    {
        CachedStage? oldStage;
        IStage newStage;
        if (CacheProvider.TryGetStages(model.GuildId, out var cache) && cache.TryGetValue(model.Id, out var stage))
        {
            newStage = stage;
            oldStage = (stage.Clone() as CachedStage)!;
            newStage.Update(model);
        }
        else
        {
            oldStage = null;
            newStage = new TransientStage(Client, model);
        }

        var e = new StageUpdatedEventArgs(oldStage, newStage);
        return new(e);
    }
}
