namespace Disqord
{
    /// <summary>
    ///     Represents an entity that can be identified with a custom ID.
    /// </summary>
    public interface ICustomIdentifiable
    {
        /// <summary>
        ///     Gets the custom ID of this entity.
        /// </summary>
        string CustomId { get; }
    }
}
