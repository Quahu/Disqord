using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class StageCreateDispatchHandler : DispatchHandler<StageInstanceJsonModel, StageCreatedEventArgs>
{
    public override ValueTask<StageCreatedEventArgs?> HandleDispatchAsync(IShard shard, StageInstanceJsonModel model)
    {
        IStage stage;
        if (CacheProvider.TryGetStages(model.GuildId, out var cache))
        {
            stage = new CachedStage(Client, model);
            cache.Add(stage.Id, (stage as CachedStage)!);
        }
        else
        {
            stage = new TransientStage(Client, model);
        }

        var e = new StageCreatedEventArgs(stage);
        return new(e);
    }
}
