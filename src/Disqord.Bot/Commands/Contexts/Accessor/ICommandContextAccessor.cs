namespace Disqord.Bot.Commands;

/// <summary>
///     Represents a type that provides access to the current execution scope's command context.
/// </summary>
public interface ICommandContextAccessor
{
    /// <summary>
    ///     Gets or sets the command context for this execution scope.
    /// </summary>
    /// <remarks>
    ///     <b>Can be <see langword="null"/> if accessed prior to execution.</b>
    /// </remarks>
    public IDiscordCommandContext Context { get; set; }
}
