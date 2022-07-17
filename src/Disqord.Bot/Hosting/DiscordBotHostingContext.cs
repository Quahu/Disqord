using System.Collections.Generic;
using Disqord.Bot.Commands.Text;
using Disqord.Hosting;

namespace Disqord.Bot.Hosting;

public class DiscordBotHostingContext : DiscordClientHostingContext
{
    /// <summary>
    ///     Gets or sets whether to allow mentioning the bot as a prefix for command execution.
    ///     Defaults to <see langword="true"/>.
    /// </summary>
    /// <remarks>
    ///     This property is ignored if a custom <see cref="IPrefixProvider"/> is registered.
    /// </remarks>
    public virtual bool UseMentionPrefix { get; set; } = true;

    /// <summary>
    ///     Gets or sets the <see cref="string"/> prefixes for command execution.
    /// </summary>
    /// <remarks>
    ///     This property is ignored if a custom <see cref="IPrefixProvider"/> is registered.
    /// </remarks>
    public virtual IEnumerable<string>? Prefixes { get; set; }

    /// <inheritdoc cref="DiscordBotBaseConfiguration.OwnerIds"/>
    public virtual IEnumerable<Snowflake>? OwnerIds { get; set; }

    /// <inheritdoc cref="DiscordBotBaseConfiguration.ApplicationId"/>
    public Snowflake? ApplicationId { get; set; }
}
