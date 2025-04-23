using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class MessagePollVoteRemoveDispatchHandler : DispatchHandler<MessagePollVoteRemoveJsonModel, PollVoteRemovedEventArgs>
{
    public override ValueTask<PollVoteRemovedEventArgs?> HandleDispatchAsync(IShard shard, MessagePollVoteRemoveJsonModel model)
    {
        var e = new PollVoteRemovedEventArgs(model.GuildId.GetValueOrNullable(), model.UserId, model.ChannelId, model.MessageId, model.AnswerId);
        return new(e);
    }
}
