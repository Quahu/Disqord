using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class AutoModerationActionExecutionDispatchHandler : DispatchHandler<AutoModerationActionExecutionJsonModel, AutoModerationActionExecutedEventArgs>
{
    public override ValueTask<AutoModerationActionExecutedEventArgs?> HandleDispatchAsync(IShard shard, AutoModerationActionExecutionJsonModel model)
    {
        var action = new TransientAutoModerationAction(model.Action);
        var e = new AutoModerationActionExecutedEventArgs(model.GuildId, model.UserId, model.ChannelId.GetValueOrNullable(), model.MessageId.GetValueOrNullable(),
            model.RuleId, model.RuleTrigger, model.AlertSystemMessageId.GetValueOrNullable(), action, model.Content,
            model.MatchedKeyword, model.MatchedContent);
        return new(e);
    }
}
