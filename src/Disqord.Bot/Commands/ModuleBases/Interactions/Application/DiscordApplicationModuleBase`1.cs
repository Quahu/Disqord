using Disqord.Bot.Commands.Interaction;

namespace Disqord.Bot.Commands.Application;

/// <summary>
///     Represents a module base for application commands.
/// </summary>
/// <typeparam name="TContext"> The command context type. </typeparam>
public abstract class DiscordApplicationModuleBase<TContext> : DiscordInteractionModuleBase<TContext>
    where TContext : IDiscordApplicationCommandContext
{
    private protected DiscordApplicationModuleBase()
    { }
}
