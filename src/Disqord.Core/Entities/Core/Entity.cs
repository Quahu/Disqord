namespace Disqord
{
    /// <summary>
    ///     Represents a Discord entity.
    /// </summary>
    public abstract class Entity : IEntity
    {
        /// <inheritdoc/>
        public IClient Client => _client ?? throw new EntityNotManagedException();

        private readonly IClient _client;

        /// <summary>
        ///     Instantiates a new entity from the provided client.
        /// </summary>
        /// <param name="client"> The client that manages this entity. </param>
        protected Entity(IClient client)
        {
            _client = client;
        }
    }
}
