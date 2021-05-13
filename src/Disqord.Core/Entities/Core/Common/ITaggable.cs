namespace Disqord
{
    /// <summary>
    ///     Represents a type with a tag. 
    ///     E.g. a user (<c>Clyde#0000</c>), a channel (<c>#general</c>).
    /// </summary>
    public interface ITaggable : IEntity
    {
        /// <summary>
        ///     Gets the tag of this object.
        /// </summary>
        string Tag { get; }
    }
}
