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
        await Task.Delay(Delay, cancellationToken).ConfigureAwait(false);
        await message.DeleteAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
