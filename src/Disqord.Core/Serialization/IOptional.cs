namespace Disqord.Serialization
{
    /// <summary>
    ///     Represents the non-generic version of <see cref="Optional{T}"/>.
    ///     Simplifies serialization.
    /// </summary>
    public interface IOptional
    {
        /// <summary>
        ///     Gets whether this <see cref="IOptional"/> has a value.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        ///     Gets the value of this <see cref="IOptional"/>.
        /// </summary>
        object? Value { get; }
    }
}
