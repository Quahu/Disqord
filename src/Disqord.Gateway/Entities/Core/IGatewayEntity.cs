namespace Disqord.Gateway
{
    /// <summary>
    ///     Represents a gateway Discord entity.
    /// </summary>
    public interface IGatewayEntity : IEntity
    {
        /// <inheritdoc cref="IEntity.Client"/>
        new IGatewayClient Client { get; }

        IClient IEntity.Client => Client;
    }
}
