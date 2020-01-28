namespace Disqord.Rest
{
    internal static class RestFetchable
    {
        public static RestFetchable<T> Create<T>(RestDiscordClient client, RestFetchableImpl<T>.RestFetchableDelegate factory)
            where T : RestDiscordEntity
            => new RestFetchableImpl<T>(client, factory);

        public static RestFetchable<T> Create<T>(RestDiscordClient client, T value, RestFetchableImpl<T>.RestFetchableDelegate factory)
            where T : RestDiscordEntity
            => new RestFetchableImpl<T>(client, value, factory);

        public static RestFetchable<T> Create<T, TState>(TState state, RestFetchableImpl<T, TState>.RestFetchableDelegate factory)
            where T : RestDiscordEntity
            => new RestFetchableImpl<T, TState>(state, factory);

        public static RestFetchable<T> Create<T, TState>(TState state, T value, RestFetchableImpl<T, TState>.RestFetchableDelegate factory)
            where T : RestDiscordEntity
            => new RestFetchableImpl<T, TState>(state, value, factory);
    }
}
