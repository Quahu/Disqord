namespace Disqord.Rest
{
    public abstract class RestDiscordEntity : IDiscordEntity
    {
        public RestDiscordClient Client { get; }

        IRestDiscordClient IDiscordEntity.Client => Client;

        internal RestDiscordEntity(RestDiscordClient client)
        {
            Client = client;
        }
    }
}
