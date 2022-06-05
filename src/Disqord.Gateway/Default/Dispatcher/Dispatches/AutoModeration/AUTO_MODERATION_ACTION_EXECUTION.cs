using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;

namespace Disqord.Gateway.Default.Dispatcher
{
    public class AutoModerationActionExecutionHandler : Handler<AutoModerationActionExecutionJsonModel, AutoModerationActionExecutedEventArgs>
    {
        public override ValueTask<AutoModerationActionExecutedEventArgs> HandleDispatchAsync(IGatewayApiClient shard, AutoModerationActionExecutionJsonModel model)
        {
            var action = new TransientAutoModerationAction(model.Action);
            var e = new AutoModerationActionExecutedEventArgs(model.GuildId, model.ChannelId, model.MessageId,
                model.RuleId, model.RuleTriggerType, model.AlertSystemMessageId, action, model.Content,
                model.MatchedKeyword, model.MatchedContent);
            return new(e);
        }
    }
}
