using System;

namespace Disqord.Bot.Commands;

public abstract class DiscordResponseCommandResult : DiscordCommandResult<IDiscordCommandContext, IUserMessage>, IDisposable
{
    public LocalMessageBase Message { get; protected set; }

    protected DiscordResponseCommandResult(IDiscordCommandContext context, LocalMessageBase message)
        : base(context)
    {
        Message = message;
    }

    public void Dispose()
    {
        foreach (var attachment in Message._attachments)
        {
            attachment?.Dispose();
        }
    }
}
