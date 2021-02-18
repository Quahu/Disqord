namespace Disqord
{
    /// <summary>
    ///     Represents an entity that can be metioned.
    ///     E.g. a user (<c>&lt;@183319356489465856&gt;</c>).
    /// </summary>
    public interface IMentionable : IEntity
    {
        /// <summary>
        ///     Gets the mention of this entity.
        /// </summary>
        string Mention { get; }
    }
}
