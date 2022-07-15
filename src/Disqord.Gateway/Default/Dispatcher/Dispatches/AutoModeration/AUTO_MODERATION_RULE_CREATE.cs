using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class AutoModerationRuleCreateHandler : Handler<AutoModerationRuleJsonModel, AutoModerationRuleCreatedEventArgs>
    {
        public override ValueTask<AutoModerationRuleCreatedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, AutoModerationRuleJsonModel model)
        {
            var rule = new TransientAutoModerationRule(Client, model);
            var e = new AutoModerationRuleCreatedEventArgs(rule);
            return new(e);
        }
    }
}
