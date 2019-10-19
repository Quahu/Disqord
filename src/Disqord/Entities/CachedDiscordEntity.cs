namespace Disqord
{
    public abstract class CachedDiscordEntity : IDiscordEntity
    {
        public DiscordClient Client { get; }

        IRestDiscordClient IDiscordEntity.Client => Client;

        internal CachedDiscordEntity(DiscordClient client)
        {
            Client = client;
        }
    }
}
