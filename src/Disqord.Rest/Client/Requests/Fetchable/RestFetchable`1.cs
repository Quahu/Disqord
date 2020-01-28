using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public abstract class RestFetchable<T> : IEquatable<T>
        where T : RestDiscordEntity
    {
        public abstract bool IsFetched { get; }

        public abstract T Value { get; internal set; }

        public abstract Task<T> FetchAsync(RestRequestOptions options = null);

        public abstract ValueTask<T> GetAsync(RestRequestOptions options = null);

        internal RestFetchable()
        { }

        public override string ToString() => IsFetched
            ? Value.ToString()
            : "<not fetched>";

        public override bool Equals(object obj) => obj is T other
            ? Equals(other)
            : base.Equals(obj);

        public bool Equals(T other) => IsFetched
            ? Value.Equals(other)
            : other == null;

        public override int GetHashCode() => IsFetched
            ? Value.GetHashCode()
            : -1;

        public static bool operator ==(RestFetchable<T> left, T right)
            => left.Equals(right);

        public static bool operator !=(RestFetchable<T> left, T right)
            => !left.Equals(right);
    }
}
