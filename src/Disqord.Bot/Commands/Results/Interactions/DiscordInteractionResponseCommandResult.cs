using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Bot.Commands.Interaction;

public class DiscordInteractionResponseCommandResult : DiscordResponseCommandResult
{
    public DiscordInteractionResponseCommandResult(IDiscordInteractionCommandContext context, LocalInteractionMessageResponse message)
        : base(context, message)
    { }

    public new TaskAwaiter<IUserMessage> GetAwaiter()
    {
        return ExecuteWithResultAsync().GetAwaiter();
    }

    public override Task<IUserMessage> ExecuteWithResultAsync(CancellationToken cancellationToken = default)
    {
        var context = Guard.IsAssignableToType<IDiscordInteractionCommandContext>(Context);
        var message = Guard.IsAssignableToType<LocalInteractionMessageResponse>(Message);
        return Guard.IsAssignableToType<Task<IUserMessage>>(context.SendMessageAsync(message, cancellationToken: cancellationToken));
    }
}
