namespace Disqord
{
    /// <summary>
    ///     Represents a type with a name.
    ///     E.g. a user (<c>Clyde</c>) or a guild (<c>The Lab</c>).
    /// </summary>
    public interface INamable
    {
        /// <summary>
        ///     Gets the name of this object.
        /// </summary>
        string Name { get; }
    }
}
