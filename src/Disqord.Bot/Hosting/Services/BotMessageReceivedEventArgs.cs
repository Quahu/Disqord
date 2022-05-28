using Disqord.Gateway;

namespace Disqord.Bot.Hosting;

/// <inheritdoc/>
public class BotMessageReceivedEventArgs : MessageReceivedEventArgs
{
    /// <summary>
    ///     Gets or sets whether to process commands for this message received event.
    ///     Defaults to <see langword="true"/>.
    ///     Set this property to <see langword="false"/> to disable redundant processing of messages
    ///     deleted by, for example, a message filtering service.
    /// </summary>
    /// <example>
    ///     Disabling processing of commands for messages in a specific channel.
    ///     <code>
    ///     protected override ValueTask OnMessageReceived(BotMessageReceivedEventArgs e)
    ///     {
    ///         e.ProcessCommands = e.ChannelId != ignoredChannelId;
    ///         return default;
    ///     }
    ///     </code>
    /// </example>
    /// <remarks>
    ///     Although this property is provided in both <see cref="DiscordBotService.OnMessageReceived(BotMessageReceivedEventArgs)"/>
    ///     and <see cref="DiscordBotService.OnNonCommandReceived"/>, setting it only has an effect in the former.
    ///     The latter method can retrieve it to know whether the processing was not performed due to the property being set to <see langword="false"/>
    ///     or because the prefix was missing etc.
    /// </remarks>
    public bool ProcessCommands { get; set; } = true;

    public BotMessageReceivedEventArgs(
        MessageReceivedEventArgs e)
        : base(e.Message, e.Channel, e.Member)
    { }
}