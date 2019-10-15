using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal delegate Task<T> RestDownloadableDelegate<T>(RestRequestOptions options = null);

    // TODO: client closures?
    public class RestDownloadable<T> : IEquatable<T> where T : class
    {
        public bool HasValue => Value != null;

        public T Value { get; private set; }

        private readonly RestDownloadableDelegate<T> _delegate;

        internal RestDownloadable(T value, RestDownloadableDelegate<T> func)
        {
            _delegate = func;
            SetValue(value);
        }

        internal RestDownloadable(RestDownloadableDelegate<T> func)
        {
            _delegate = func;
        }

        internal void SetValue(T value)
            => Value = value;

        public async Task<T> DownloadAsync(RestRequestOptions options = null)
        {
            var value = await _delegate(options).ConfigureAwait(false);
            SetValue(value);
            return value;
        }

        public async Task<T> GetOrDownloadAsync(RestRequestOptions options = null)
        {
            if (HasValue)
                return Value;

            var value = await _delegate(options).ConfigureAwait(false);
            SetValue(value);
            return value;
        }

        public override string ToString()
            => HasValue
                ? Value.ToString()
                : "<not downloaded>";

        public override bool Equals(object obj)
            => obj is T other
                ? Equals(other)
                : base.Equals(obj);

        public bool Equals(T other)
            => HasValue
                ? Value.Equals(other)
                : other == null;

        public override int GetHashCode()
            => HasValue
                ? Value.GetHashCode()
                : -1;

        public static bool operator ==(RestDownloadable<T> left, T right)
            => left.Equals(right);

        public static bool operator !=(RestDownloadable<T> left, T right)
            => !left.Equals(right);
    }
}
