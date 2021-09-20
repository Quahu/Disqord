using System;

namespace Disqord
{
    /// <summary>
    ///     Defines static utility methods for working with <see cref="Optional{T}"/>.
    /// </summary>
    public static class Optional
    {
        /// <summary>
        ///     Creates a new <see cref="Optional{T}"/> from the provided value.
        /// </summary>
        /// <param name="value"> The value to create the optional from. </param>
        /// <typeparam name="T"> The type of the value to create the optional from. </typeparam>
        /// <returns>
        ///     The provided value wrapped in an <see cref="Optional{T}"/>.
        /// </returns>
        public static Optional<T> Create<T>(T value)
            => new(value);

        /// <summary>
        ///     Creates a new <see cref="Optional{T}"/> from the provided nullable value type.
        ///     If <paramref name="value"/> is <see langword="null"/> the created optional will have no value.
        /// </summary>
        /// <param name="value"> The value to create the optional from. </param>
        /// <typeparam name="T"> The type of the value to create the optional from. </typeparam>
        /// <returns>
        ///     The provided value wrapped in an <see cref="Optional{T}"/>.
        /// </returns>
        public static Optional<T> FromNullable<T>(T? value)
            where T : struct
            => value ?? Optional<T>.Empty;

        /// <summary>
        ///     Creates a new <see cref="Optional{T}"/> from the provided nullable reference type.
        ///     If <paramref name="value"/> is <see langword="null"/> the created optional will have no value.
        /// </summary>
        /// <param name="value"> The value to create the optional from. </param>
        /// <typeparam name="T"> The type of the value to create the optional from. </typeparam>
        /// <returns>
        ///     The provided value wrapped in an <see cref="Optional{T}"/>.
        /// </returns>
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

        public static TNew ConvertOrDefault<TOld, TState, TNew>(Optional<TOld> optional, Func<TOld, TState, TNew> converter, TState state)
            => optional.HasValue ? converter(optional.Value, state) : default;

        public static TNew ConvertOrDefault<TOld, TState, TNew>(Optional<TOld> optional, Func<TOld, TState, TNew> converter, TState state, TNew defaultValue)
            => optional.HasValue ? converter(optional.Value, state) : defaultValue;

        public static Optional<T> Conditional<T>(bool condition, T value)
            => condition ? value : Optional<T>.Empty;

        public static Optional<T> Conditional<T, TState>(bool condition, Func<TState, T> factory, TState state)
            => condition ? factory(state) : Optional<T>.Empty;
    }
}
