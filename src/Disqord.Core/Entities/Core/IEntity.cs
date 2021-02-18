namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        ///     Gets the client that created this entity.
        ///     Throws in scenarios where the entity is not managed by a client.
        ///     E.g. <c>local</c> entities.
        /// </summary>
        /// <exception cref="EntityNotManagedException"> Thrown when this entity is not tied to a client. </exception>
        IClient Client { get; }
    }
}
