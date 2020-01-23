namespace Disqord.Rest
{
    /// <summary>
    ///     Represents a REST Discord entity.
    /// </summary>
    public abstract class RestDiscordEntity : IDiscordEntity
    {
        /// <inheritdoc cref="IDiscordEntity.Client"/>
        public RestDiscordClient Client { get; }

        IRestDiscordClient IDiscordEntity.Client => Client;

        internal RestDiscordEntity(RestDiscordClient client)
        {
            Client = client;
        }
    }
}
