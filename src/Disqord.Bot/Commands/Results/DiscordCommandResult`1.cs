using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot.Commands;

public abstract class DiscordCommandResult<TContext> : IDiscordCommandResult
    where TContext : IDiscordCommandContext
{
    public TContext Context { get; }

    protected DiscordCommandResult(TContext context)
    {
        Context = context;
    }

    public virtual TaskAwaiter GetAwaiter()
    {
        return ExecuteAsync().GetAwaiter();
    }

    public abstract Task ExecuteAsync(CancellationToken cancellationToken = default);
}
