using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    internal sealed class FetchableSnowflakeOptionalImpl<TCached, TRest, TShared> : FetchableSnowflakeOptional<TShared>
        where TCached : CachedSnowflakeEntity, TShared
        where TRest : RestSnowflakeEntity, TShared
        where TShared : ISnowflakeEntity
    {
        public override bool HasValue => _value != null;

        public override bool IsCached => _value is TCached;

        public override bool IsFetched => _value is TRest;

        public override TShared Value => _value ?? throw new InvalidOperationException(
            "This fetchable snowflake optional has no value.");

        private TShared _value;

        private readonly RestFetchable<TRest> _fetchable;

        public FetchableSnowflakeOptionalImpl(Snowflake id, TCached value, RestFetchable<TRest> fetchable)
            : base(id)
        {
            _value = value;
            _fetchable = fetchable;
        }

        public override ValueTask<TShared> GetAsync(RestRequestOptions options = null) => _value != null
            ? new ValueTask<TShared>(_value)
            : new ValueTask<TShared>(FetchAsync(options));

        public override async Task<TShared> FetchAsync(RestRequestOptions options = null)
            => _value = await _fetchable.FetchAsync(options).ConfigureAwait(false);
    }
}
