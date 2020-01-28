using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public abstract class FetchableSnowflakeOptional<T>
        where T : ISnowflakeEntity
    {
        public Snowflake Id { get; }

        public abstract bool HasValue { get; }

        public abstract bool IsCached { get; }

        public abstract bool IsFetched { get; }

        public abstract T Value { get; }

        internal FetchableSnowflakeOptional(Snowflake id)
        {
            Id = id;
        }

        public abstract Task<T> FetchAsync(RestRequestOptions options = null);

        public abstract ValueTask<T> GetAsync(RestRequestOptions options = null);
    }
}
