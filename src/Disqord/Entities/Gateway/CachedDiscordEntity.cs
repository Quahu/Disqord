namespace Disqord
{
    public abstract class CachedDiscordEntity : IDiscordEntity
    {
        public DiscordClientBase Client { get; }

        IRestDiscordClient IDiscordEntity.Client => Client;

        internal CachedDiscordEntity(DiscordClientBase client)
        {
            Client = client;
        }
    }
}
