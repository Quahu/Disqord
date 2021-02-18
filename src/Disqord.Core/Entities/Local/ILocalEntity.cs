namespace Disqord.Entities.Local
{
    /// <summary>
    ///     Represents an entity is used to send user data to the Discord API and is not managed by clients.
    /// </summary>
    public interface ILocalEntity : IEntity
    {
        /// <summary>
        ///     Returns <see langword="null"/>.
        /// </summary>
        IClient IEntity.Client => null;
    }
}
