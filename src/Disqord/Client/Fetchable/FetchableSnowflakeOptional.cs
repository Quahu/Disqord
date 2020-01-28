using Disqord.Rest;

namespace Disqord
{
    internal static class FetchableSnowflakeOptional
    {
        public static FetchableSnowflakeOptional<TShared> Create<TCached, TRest, TShared>(Snowflake id, TCached value, RestFetchable<TRest> fetchable)
            where TCached : CachedSnowflakeEntity, TShared
            where TRest : RestSnowflakeEntity, TShared
            where TShared : ISnowflakeEntity
            => new FetchableSnowflakeOptionalImpl<TCached, TRest, TShared>(id, value, fetchable);
    }
}
