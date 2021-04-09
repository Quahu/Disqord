using System;

namespace Disqord
{
    public static class Optional
    {
        public static Optional<T> Create<T>(T value)
            => new(value);

        public static Optional<T> FromNullable<T>(T? value)
            where T : struct
            => value ?? Optional<T>.Empty;

        public static Optional<T> FromNullable<T>(T value)
            where T : class
            => value ?? Optional<T>.Empty;

        public static Optional<TNew> Convert<TOld, TNew>(Optional<TOld> optional, Converter<TOld, TNew> converter)
            => optional.HasValue
                ? optional.Value != null
                    ? converter(optional.Value)
                    : default
                : Optional<TNew>.Empty;

        public static TNew ConvertOrDefault<TOld, TNew>(Optional<TOld> optional, Converter<TOld, TNew> converter)
            => optional.HasValue ? converter(optional.Value) : default;

        public static TNew ConvertOrDefault<TOld, TNew>(Optional<TOld> optional, Converter<TOld, TNew> converter, TNew defaultValue)
            => optional.HasValue ? converter(optional.Value) : defaultValue;

        public static Optional<T> Conditional<T>(bool condition, T value)
            => condition ? value : Optional<T>.Empty;
        
        public static Optional<T> Conditional<T, TState>(bool condition, Func<TState, T> factory, TState state)
            => condition ? factory(state) : Optional<T>.Empty;
    }
}
