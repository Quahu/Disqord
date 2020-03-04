namespace Disqord.Rest
{
    internal class RestFetchableImpl<T> : RestFetchableImpl<T, RestDiscordClient>
        where T : RestDiscordEntity
    {
        public RestFetchableImpl(RestDiscordClient client, T value, RestFetchableDelegate func)
            : base(client, value, func)
        { }

        public RestFetchableImpl(RestDiscordClient client, RestFetchableDelegate func)
            : base(client, func)
        { }
    }
}
