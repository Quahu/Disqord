using System.Threading.Tasks;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Models;
using Qommon;

namespace Disqord.Gateway.Default.Dispatcher;

public class MessagePollVoteAddDispatchHandler : DispatchHandler<MessagePollVoteAddJsonModel, PollVoteAddedEventArgs>
{
    public override ValueTask<PollVoteAddedEventArgs?> HandleDispatchAsync(IShard shard, MessagePollVoteAddJsonModel model)
    {
        var e = new PollVoteAddedEventArgs(model.GuildId.GetValueOrNullable(), model.UserId, model.ChannelId, model.MessageId, model.AnswerId);
        return new(e);
    }
}
