using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord.Bot.Commands.Text;

public class DiscordTextResponseCommandResult : DiscordResponseCommandResult
{
    public DiscordTextResponseCommandResult(IDiscordCommandContext context, LocalMessage message)
        : base(context, message)
    { }

    public new TaskAwaiter<IUserMessage> GetAwaiter()
    {
        return ExecuteWithResultAsync().GetAwaiter();
    }

    public override Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteWithResultAsync(cancellationToken);
    }

    public override Task<IUserMessage> ExecuteWithResultAsync(CancellationToken cancellationToken = default)
    {
        return Context.Bot.SendMessageAsync(Context.ChannelId, (Message as LocalMessage)!, cancellationToken: cancellationToken);
    }
}
