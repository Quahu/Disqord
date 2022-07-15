using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class AutoModerationRuleUpdateHandler : Handler<AutoModerationRuleJsonModel, AutoModerationRuleUpdatedEventArgs>
    {
        public override ValueTask<AutoModerationRuleUpdatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, AutoModerationRuleJsonModel model)
        {
            var rule = new TransientAutoModerationRule(Client, model);
            var e = new AutoModerationRuleUpdatedEventArgs(rule);
            return new(e);
        }
    }
}
