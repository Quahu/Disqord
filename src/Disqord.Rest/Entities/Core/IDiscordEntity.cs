namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity.
    /// </summary>
    public interface IDiscordEntity
    {
        /// <summary>
        ///     Gets the <see cref="IDiscordClient"/> that created this <see cref="IDiscordEntity"/>.
        /// </summary>
        IDiscordClient Client { get; }
    }
}
