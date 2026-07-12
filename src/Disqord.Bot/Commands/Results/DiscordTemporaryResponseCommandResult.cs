using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Disqord.Bot.Commands;

public class DiscordTemporaryResponseCommandResult(DiscordResponseCommandResult result, TimeSpan delay)
    : DiscordCommandResult<IDiscordCommandContext>(result.Context)
{
    public DiscordResponseCommandResult Result { get; protected set; } = result;

    public TimeSpan Delay { get; protected set; } = delay;

    public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var message = await Result.ExecuteWithResultAsync(cancellationToken).ConfigureAwait(false);
        _ = DeleteAfterDelayAsync(message, cancellationToken);
    }

    private async Task DeleteAfterDelayAsync(IUserMessage message, CancellationToken cancellationToken)
    {
        try
        {
            await Task.Delay(Delay, cancellationToken).ConfigureAwait(false);

            await Result.DeleteResponseAsync(message, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        { }
        catch (Exception ex)
        {
            Context.Bot.Logger.LogError(ex, "An exception occurred while deleting the temporary response message ({0}).", message.Id);
        }
    }
}
