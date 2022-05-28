using System.ComponentModel;
using System.Threading.Tasks;
using Disqord.Bot.Commands;
using Disqord.Gateway;

namespace Disqord.Bot.Hosting;

public abstract partial class DiscordBotService
{
    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected internal sealed override ValueTask OnMessageReceived(MessageReceivedEventArgs e)
        => default;

    /// <summary>
    ///     Fires when a message is received.
    ///     Methods overriding this will run before any command processing by the bot client happens
    ///     and are able to disable it for, for example, messages deleted by a message filtering service.
    /// </summary>
    /// <param name="e"> The event data. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the handler work.
    /// </returns>
    /// <seealso cref="BotMessageReceivedEventArgs.ProcessCommands"/>
    protected internal virtual ValueTask OnMessageReceived(BotMessageReceivedEventArgs e)
        => default;

    /// <summary>
    ///     Fires after a message is received and is known not to be a command, i.e.
    ///     messages sent by bots, messages without a prefix, system messages, etc.
    ///     This also fires for messages with <see cref="BotMessageReceivedEventArgs.ProcessCommands"/> set to <see langword="false"/>.
    /// </summary>
    /// <param name="e"> The event data. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the handler work.
    /// </returns>
    /// <seealso cref="BotMessageReceivedEventArgs.ProcessCommands"/>
    protected internal virtual ValueTask OnNonCommandReceived(BotMessageReceivedEventArgs e)
        => default;

    /// <summary>
    ///     Fires after a message is received and command processing yields a not found result.
    ///     This does not fire for messages that had <see cref="OnNonCommandReceived"/> fire instead.
    /// </summary>
    /// <param name="context"> The command context used for command execution. </param>
    /// <returns>
    ///     A <see cref="ValueTask"/> representing the handler work.
    /// </returns>
    protected internal virtual ValueTask OnCommandNotFound(IDiscordCommandContext context)
        => default;
}
