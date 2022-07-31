using System;
using System.Globalization;
using System.Threading.Tasks;
using Disqord.Bot.Commands.Text;
using Disqord.Gateway;

namespace Disqord.Bot;

public abstract partial class DiscordBotBase
{
    /// <summary>
    ///     Checks if the received message should be processed.
    /// </summary>
    /// <remarks>
    ///     By default ensures the message author is not a bot.
    /// </remarks>
    /// <param name="message"> The message to check. </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}"/> representing the work where the result indicates whether the message should be processed.
    /// </returns>
    protected virtual ValueTask<bool> OnMessage(IGatewayUserMessage message)
    {
        return new(!message.Author.IsBot);
    }

    /// <summary>
    ///     Creates an <see cref="IDiscordTextCommandContext"/> from the provided parameters.
    /// </summary>
    /// <param name="prefix"> The prefix found in the message. </param>
    /// <param name="input"> The input possibly containing the command. </param>
    /// <param name="message"> The message possibly containing the command. </param>
    /// <param name="channel"> The optional cached channel the message was sent in. </param>
    /// <returns>
    ///     An <see cref="IDiscordTextCommandContext"/> or an <see cref="IDiscordTextGuildCommandContext"/> for guild messages.
    /// </returns>
    public virtual IDiscordTextCommandContext CreateTextCommandContext(IPrefix prefix, ReadOnlyMemory<char> input, IGatewayUserMessage message, IMessageGuildChannel? channel)
    {
        // TODO: culture?
        var context = message.GuildId != null
            ? new DiscordTextGuildCommandContext(this, prefix, message, channel, CultureInfo.InvariantCulture, null)
            : new DiscordTextCommandContext(this, prefix, message, CultureInfo.InvariantCulture, null);

        context.InputString = input;

        return context;
    }
}
