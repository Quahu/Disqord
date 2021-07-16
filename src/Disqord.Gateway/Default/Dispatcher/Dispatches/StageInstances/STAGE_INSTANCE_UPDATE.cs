using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class StageInstanceUpdateHandler : Handler<StageInstanceJsonModel, StageInstanceUpdatedEventArgs>
    {
        public override ValueTask<StageInstanceUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, StageInstanceJsonModel model)
        {
            CachedStageInstance oldStageInstance;
            IStageInstance newStageInstance;
            if (CacheProvider.TryGetStageInstances(model.GuildId, out var cache) && cache.TryGetValue(model.Id, out var stageInstance))
            {
                newStageInstance = stageInstance;
                oldStageInstance = stageInstance.Clone() as CachedStageInstance;
                newStageInstance.Update(model);
            }
            else
            {
                oldStageInstance = null;
                newStageInstance = new TransientStageInstance(Client, model);
            }

            var e = new StageInstanceUpdatedEventArgs(oldStageInstance, newStageInstance);
            return new(e);
        }
    }
}