using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Qmmands;

namespace Disqord.Bot.Commands;

public interface IDiscordCommandResult : IResult
{
    bool IResult.IsSuccessful => true;

    string? IResult.FailureReason => null;

    TaskAwaiter GetAwaiter();

    Task ExecuteAsync();
}
