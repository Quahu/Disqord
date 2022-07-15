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
    ///     Can be <see langword="null"/>
    /// </remarks>
    public IDiscordCommandContext Context { get; set; }
}
