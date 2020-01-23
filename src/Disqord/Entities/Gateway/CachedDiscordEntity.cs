using Disqord.Rest;

namespace Disqord
{
    /// <summary>
    ///     Represents a cached Discord entity.
    /// </summary>
    public abstract class CachedDiscordEntity : IDiscordEntity
    {
        /// <inheritdoc cref="IDiscordEntity.Client"/>
        public DiscordClientBase Client { get; }

        IRestDiscordClient IDiscordEntity.Client => Client;

        internal CachedDiscordEntity(DiscordClientBase client)
        {
            Client = client;
        }
    }
}
