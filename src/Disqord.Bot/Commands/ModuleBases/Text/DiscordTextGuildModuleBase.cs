namespace Disqord.Bot.Commands.Text;

/// <inheritdoc/>
/// <remarks>
///     This base restricts the execution of commands to guilds
///     and changes the command context type accordingly.
/// </remarks>
[RequireGuild]
public abstract class DiscordTextGuildModuleBase : DiscordTextModuleBase<IDiscordTextGuildCommandContext>
{ }
