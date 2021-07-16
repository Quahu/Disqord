using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class StageInstanceDeleteHandler : Handler<StageInstanceJsonModel, StageInstanceDeletedEventArgs>
    {
        public override ValueTask<StageInstanceDeletedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, StageInstanceJsonModel model)
        {
            IStageInstance stageInstance;
            if (CacheProvider.TryGetStageInstances(model.GuildId, out var cache) && cache.TryRemove(model.Id, out var cachedStageInstance))
            {
                stageInstance = cachedStageInstance;
            }
            else
            {
                stageInstance = new TransientStageInstance(Client, model);
            }

            var e = new StageInstanceDeletedEventArgs(stageInstance);
            return new(e);
        }
    }
}