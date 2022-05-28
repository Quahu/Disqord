using System.Runtime.CompilerServices;
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

    public override Task ExecuteAsync()
    {
        return ExecuteWithResultAsync();
    }

    public override Task<IUserMessage> ExecuteWithResultAsync()
    {
        return Context.Bot.SendMessageAsync(Context.ChannelId, (Message as LocalMessage)!, cancellationToken: Context.CancellationToken);
    }
}
