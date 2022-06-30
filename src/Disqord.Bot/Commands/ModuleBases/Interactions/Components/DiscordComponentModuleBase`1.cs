using Disqord.Bot.Commands.Interaction;

namespace Disqord.Bot.Commands.Components;

/// <summary>
///     Represents a module base for component commands.
/// </summary>
/// <typeparam name="TContext"> The command context type. </typeparam>
public abstract class DiscordComponentModuleBase<TContext> : DiscordInteractionModuleBase<TContext>
    where TContext : IDiscordComponentCommandContext
{
    private protected DiscordComponentModuleBase()
    { }
}
