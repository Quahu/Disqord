namespace Disqord
{
    /// <summary>
    ///     Represents an entity with a name.
    ///     E.g. a user (<c>Clyde</c>) or a guild (<c>The Lab</c>).
    /// </summary>
    public interface INamable : IEntity
    {
        /// <summary>
        ///     Gets the name of this entity.
        /// </summary>
        string Name { get; }
    }
}
