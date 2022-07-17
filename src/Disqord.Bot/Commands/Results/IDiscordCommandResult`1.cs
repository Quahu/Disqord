using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Bot.Commands;

public interface IDiscordCommandResult<TResult> : IDiscordCommandResult
{
    Task<TResult> ExecuteWithResultAsync(CancellationToken cancellationToken = default);
}
