using Disqord.Gateway;
using Qmmands.Text;

namespace Disqord.Bot.Commands.Text;

public interface IDiscordTextCommandContext : IDiscordCommandContext, ITextCommandContext
{
    /// <summary>
    ///     Gets the prefix that was used to execute the command.
    /// </summary>
    IPrefix Prefix { get; }

    /// <summary>
    ///     Gets the message that triggered the command execution.
    /// </summary>
    IGatewayUserMessage Message { get; }

    IUser IDiscordCommandContext.Author => Message.Author;
}
