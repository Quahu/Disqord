namespace Disqord.Bot.Commands.Application;

/// <inheritdoc/>
/// <remarks>
///     This base restricts the execution of commands to guilds
///     and changes the command context type accordingly.
/// </remarks>
[RequireGuild]
public abstract class DiscordApplicationGuildModuleBase : DiscordApplicationModuleBase<IDiscordApplicationGuildCommandContext>
{ }
