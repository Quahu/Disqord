using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class StageInstanceCreateHandler : Handler<StageInstanceJsonModel, StageInstanceCreatedEventArgs>
    {
        public override ValueTask<StageInstanceCreatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, StageInstanceJsonModel model)
        {
            IStageInstance stageInstance;
            if (CacheProvider.TryGetStageInstances(model.GuildId, out var cache))
            {
                stageInstance = new CachedStageInstance(Client, model);
                cache.Add(stageInstance.Id, stageInstance as CachedStageInstance);
            }
            else
            {
                stageInstance = new TransientStageInstance(Client, model);
            }

            var e = new StageInstanceCreatedEventArgs(stageInstance);
            return new(e);
        }
    }
}