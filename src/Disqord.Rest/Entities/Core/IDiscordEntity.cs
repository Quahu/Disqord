namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity.
    /// </summary>
    public interface IDiscordEntity
    {
        /// <summary>
        ///     Gets the <see cref="IRestDiscordClient"/> that created this <see cref="IDiscordEntity"/>.
        /// </summary>
        IRestDiscordClient Client { get; }
    }
}
