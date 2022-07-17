using System;
using Qommon;

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
