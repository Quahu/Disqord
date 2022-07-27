using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class AutoModerationRuleUpdateDispatchHandler : DispatchHandler<AutoModerationRuleJsonModel, AutoModerationRuleUpdatedEventArgs>
{
    public override ValueTask<AutoModerationRuleUpdatedEventArgs?> HandleDispatchAsync(IShard shard, AutoModerationRuleJsonModel model)
    {
        var rule = new TransientAutoModerationRule(Client, model);
        var e = new AutoModerationRuleUpdatedEventArgs(rule);
        return new(e);
    }
}
