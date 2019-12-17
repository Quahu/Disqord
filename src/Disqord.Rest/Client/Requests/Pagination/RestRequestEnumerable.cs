using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    /// <summary>
    ///     Represents an asynchronously paged enumerable of <typeparamref name="T"/>s.
    ///     Does not support multiple enumerations.
    /// </summary>
    /// <typeparam name="T"> The <see cref="Type"/> of items in pages. </typeparam>
    public sealed class RestRequestEnumerable<T> : IAsyncEnumerable<IReadOnlyList<T>>
    {
        private readonly RestRequestEnumerator<T> _enumerator;

        internal RestRequestEnumerable(RestRequestEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public ConfiguredRestRequestEnumerable WithOptions(RestRequestOptions options)
            => new ConfiguredRestRequestEnumerable(this, options);

        /// <summary>
        ///     Flattens this <see cref="RestRequestEnumerable{T}"/> into a single read-only list.
        /// </summary>
        /// <param name="options"> The <see cref="RestRequestOptions"/> to use for requests. </param>
        /// <returns>
        ///     A flattened list of <typeparamref name="T"/>s.
        /// </returns>
        public Task<IReadOnlyList<T>> FlattenAsync(RestRequestOptions options = null)
            => FlattenAsync(CancellationToken.None, options);

        /// <summary>
        ///     Flattens this <see cref="RestRequestEnumerable{T}"/> into a single read-only list.
        /// </summary>
        /// <param name="cancellationToken"> The <see cref="CancellationToken"/> to use for requests. </param>
        /// <param name="options"> The <see cref="RestRequestOptions"/> to use for requests. </param>
        /// <returns>
        ///     A flattened list of <typeparamref name="T"/>s.
        /// </returns>
        public async Task<IReadOnlyList<T>> FlattenAsync(CancellationToken cancellationToken, RestRequestOptions options = null)
        {
            var builder = ImmutableArray.CreateBuilder<T>();
            await foreach (var items in this.WithOptions(options).WithCancellation(cancellationToken).ConfigureAwait(false))
                builder.AddRange(items);

            return builder.ToImmutable();
        }

        public RestRequestEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            => _enumerator;

        IAsyncEnumerator<IReadOnlyList<T>> IAsyncEnumerable<IReadOnlyList<T>>.GetAsyncEnumerator(CancellationToken cancellationToken)
            => _enumerator;

        public readonly struct ConfiguredRestRequestEnumerable : IAsyncEnumerable<IReadOnlyList<T>>
        {
            private readonly RestRequestEnumerable<T> _enumerable;
            private readonly RestRequestOptions _options;

            internal ConfiguredRestRequestEnumerable(RestRequestEnumerable<T> enumerable, RestRequestOptions options = null)
            {
                _enumerable = enumerable;
                _options = options;
            }

            public IAsyncEnumerator<IReadOnlyList<T>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
                => new Enumerator(_enumerable.GetAsyncEnumerator(cancellationToken), _options);

            public readonly struct Enumerator : IAsyncEnumerator<IReadOnlyList<T>>
            {
                public IReadOnlyList<T> Current => _enumerator.Current;

                private readonly RestRequestEnumerator<T> _enumerator;
                private readonly RestRequestOptions _options;

                internal Enumerator(RestRequestEnumerator<T> enumerator, RestRequestOptions options = null)
                {
                    _enumerator = enumerator;
                    _options = options;
                }

                public ValueTask<bool> MoveNextAsync()
                    => _enumerator.MoveNextAsync(_options);

                public ValueTask DisposeAsync()
                    => _enumerator.DisposeAsync();
            }
        }
    }
}
