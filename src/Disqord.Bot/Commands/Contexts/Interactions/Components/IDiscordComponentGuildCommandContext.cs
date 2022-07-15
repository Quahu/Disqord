using Disqord.Bot.Commands.Interaction;

namespace Disqord.Bot.Commands.Components;

/// <summary>
///     Represents a guild application command execution context.
/// </summary>
public interface IDiscordComponentGuildCommandContext : IDiscordComponentCommandContext, IDiscordInteractionGuildCommandContext
{ }
