using System;

namespace Disqord
{
    public static class OptionalExtensions
    {
        public static T? GetValueOrNullable<T>(this Optional<T> optional)
            where T : struct
            => optional.HasValue ? optional.Value : new T?();

        public static T GetValueOrDefault<T>(this Optional<T> optional)
            => optional.HasValue ? optional.Value : default;

        public static T GetValueOrDefault<T>(this Optional<T> optional, T value)
            => optional.HasValue ? optional.Value : value;

        public static T GetValueOrDefault<T>(this Optional<T> optional, Func<T> factory)
            => optional.HasValue ? optional.Value : (factory ?? throw new ArgumentNullException(nameof(factory)))();

        public static T GetValueOrDefault<T, TState>(this Optional<T> optional, Func<TState, T> factory, TState state)
            => optional.HasValue ? optional.Value : (factory ?? throw new ArgumentNullException(nameof(factory)))(state);
    }
}
