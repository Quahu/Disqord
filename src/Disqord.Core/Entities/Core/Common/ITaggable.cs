namespace Disqord
{
    /// <summary>
    ///     Represents an entity with a tag. 
    ///     E.g. a user (<c>Clyde#0000</c>), a channel (<c>#general</c>).
    /// </summary>
    public interface ITaggable : IEntity
    {
        /// <summary>
        ///     Gets the tag of this entity.
        /// </summary>
        string Tag { get; }
    }
}
