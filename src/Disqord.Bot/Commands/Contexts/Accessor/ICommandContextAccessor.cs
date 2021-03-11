namespace Disqord.Bot
{
    /// <summary>
    ///     Represents a type that provides access to the current execution scope's command context.
    /// </summary>
    public interface ICommandContextAccessor
    {
        /// <summary>
        ///     Gets or sets the command context for this execution scope.
        /// </summary>
        public DiscordCommandContext Context { get; set; }
    }
}
