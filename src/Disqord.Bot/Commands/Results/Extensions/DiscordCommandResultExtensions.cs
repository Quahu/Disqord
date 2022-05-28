using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Disqord.Bot.Commands;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class DiscordCommandResultExtensions
{
    public static ResultAwaitable<TResult> WithResult<TResult>(this IDiscordCommandResult<TResult> result)
        => new(result);

    public readonly struct ResultAwaitable<TResult>
    {
        public IDiscordCommandResult<TResult> Result { get; }

        public ResultAwaitable(IDiscordCommandResult<TResult> result)
        {
            Result = result;
        }

        public TaskAwaiter<TResult> GetAwaiter()
        {
            return Result.ExecuteWithResultAsync().GetAwaiter();
        }
    }
}
