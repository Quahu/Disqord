using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Bot.Commands.Interaction;

public class DiscordInteractionResponseCommandResult : DiscordResponseCommandResult
{
    public DiscordInteractionResponseCommandResult(IDiscordInteractionCommandContext context, LocalInteractionMessageResponse message)
        : base(context, message)
    { }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var context = Guard.IsAssignableToType<IDiscordInteractionCommandContext>(Context);
        var message = Guard.IsAssignableToType<LocalInteractionMessageResponse>(Message);
        return context.SendMessageAsync(message, false, cancellationToken: cancellationToken);
    }

    public override Task<IUserMessage> ExecuteWithResultAsync(CancellationToken cancellationToken = default)
    {
        var context = Guard.IsAssignableToType<IDiscordInteractionCommandContext>(Context);
        var message = Guard.IsAssignableToType<LocalInteractionMessageResponse>(Message);
        return Guard.IsAssignableToType<Task<IUserMessage>>(context.SendMessageAsync(message, true, cancellationToken: cancellationToken));
    }
}
