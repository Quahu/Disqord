using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot.Commands;

public class DiscordTemporaryResponseCommandResult : DiscordCommandResult<IDiscordCommandContext>
{
    public DiscordResponseCommandResult Result { get; protected set; }

    public TimeSpan Delay { get; protected set; }

    public DiscordTemporaryResponseCommandResult(DiscordResponseCommandResult result, TimeSpan delay)
        : base(result.Context)
    {
        Result = result;
        Delay = delay;
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var message = await Result.ExecuteWithResultAsync(cancellationToken).ConfigureAwait(false);
        _ = Task.Delay(Delay, cancellationToken).ContinueWith(static (_, state) =>
        {
            var (message, cancellationToken) = (ValueTuple<IMessage, CancellationToken>) state!;
            return message.DeleteAsync(cancellationToken: cancellationToken);
        }, (message as IMessage, cancellationToken), TaskContinuationOptions.OnlyOnRanToCompletion);
    }
}
