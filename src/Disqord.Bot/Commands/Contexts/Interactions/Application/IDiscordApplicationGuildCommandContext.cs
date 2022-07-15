using Disqord.Bot.Commands.Interaction;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents a guild application command execution context.
/// </summary>
public interface IDiscordApplicationGuildCommandContext : IDiscordApplicationCommandContext, IDiscordInteractionGuildCommandContext
{ }
