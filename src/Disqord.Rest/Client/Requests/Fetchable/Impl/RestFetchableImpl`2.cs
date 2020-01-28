using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal class RestFetchableImpl<T, TState> : RestFetchable<T>
        where T : RestDiscordEntity
    {
        internal delegate Task<T> RestFetchableDelegate(TState state, RestRequestOptions options);

        public override bool IsFetched => _value != null;

        public override T Value
        {
            get => _value ?? throw new InvalidOperationException(
                $"This fetchable has no fetched value. Call {nameof(FetchAsync)}/{nameof(GetAsync)} to populate it.");
            internal set => _value = value;
        }
        private T _value;

        private readonly TState _state;
        private readonly RestFetchableDelegate _delegate;

        internal RestFetchableImpl(TState state, T value, RestFetchableDelegate func)
            : this(state, func)
        {
            _value = value;
        }

        internal RestFetchableImpl(TState state, RestFetchableDelegate func)
        {
            _state = state;
            _delegate = func;
        }

        public override async Task<T> FetchAsync(RestRequestOptions options = null)
            => _value = await _delegate(_state, options).ConfigureAwait(false);

        public override ValueTask<T> GetAsync(RestRequestOptions options = null) => _value != null
            ? new ValueTask<T>(_value)
            : new ValueTask<T>(FetchAsync(options));
    }
}
