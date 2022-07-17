using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot.Commands;

public abstract class DiscordCommandResult<TContext, TResult> : DiscordCommandResult<TContext>, IDiscordCommandResult<TResult>
    where TContext : IDiscordCommandContext
{
    protected DiscordCommandResult(TContext context)
        : base(context)
    { }

    public abstract Task<TResult> ExecuteWithResultAsync(CancellationToken cancellationToken = default);
}
