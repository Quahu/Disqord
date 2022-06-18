namespace Disqord.Bot.Commands;

/// <summary>
///     Represents a guild interaction command execution context.
/// </summary>
public interface IDiscordInteractionGuildCommandContext : IDiscordInteractionCommandContext, IDiscordGuildCommandContext
{ }
