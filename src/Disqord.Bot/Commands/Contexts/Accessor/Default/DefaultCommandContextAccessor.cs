namespace Disqord.Bot
{
    /// <inheritdoc/>
    public class DefaultCommandContextAccessor : ICommandContextAccessor
    {
        /// <inheritdoc/>
        public DiscordCommandContext Context { get; set; }
    }
}
