namespace Disqord
{
    /// <summary>
    ///     Represents a type with a <see cref="Snowflake"/> ID.
    /// </summary>
    public interface IIdentifiable
    {
        /// <summary>
        ///     Gets the ID of this object.
        /// </summary>
        Snowflake Id { get; }
    }
}
