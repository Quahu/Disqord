using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;
using Qommon;

namespace Disqord.Bot.Commands;

public abstract class DiscordResponseCommandResult(IDiscordCommandContext context, LocalMessageBase message)
    : DiscordCommandResult<IDiscordCommandContext, IUserMessage>(context), IDisposable
{
    public LocalMessageBase Message { get; protected set; } = message;

    /// <summary>
    ///     Deletes the message previously sent by this result.
    /// </summary>
    /// <param name="message"> The message sent by this result. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    /// <returns>
    ///     A <see cref="Task"/> representing the deletion.
    /// </returns>
    protected internal virtual Task DeleteResponseAsync(IUserMessage message, CancellationToken cancellationToken = default)
    {
        return message.DeleteAsync(cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        var attachments = Message.Attachments.GetValueOrDefault();
        if (attachments != null)
        {
            foreach (var attachment in attachments)
            {
                attachment?.Dispose();
            }
        }
    }
}
