using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Models;

namespace Disqord.Gateway.Default.Dispatcher;

public class AutoModerationRuleDeleteDispatchHandler : DispatchHandler<AutoModerationRuleJsonModel, AutoModerationRuleDeletedEventArgs>
{
    public override ValueTask<AutoModerationRuleDeletedEventArgs?> HandleDispatchAsync(IShard shard, AutoModerationRuleJsonModel model)
    {
        var rule = new TransientAutoModerationRule(Client, model);
        var e = new AutoModerationRuleDeletedEventArgs(rule);
        return new(e);
    }
}
