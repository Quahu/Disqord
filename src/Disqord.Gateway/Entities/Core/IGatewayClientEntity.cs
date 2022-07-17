namespace Disqord.Gateway;

/// <summary>
///     Represents a gateway Discord entity.
/// </summary>
public interface IGatewayClientEntity : IClientEntity, IGatewayEntity
{
    /// <inheritdoc cref="IClientEntity.Client"/>
    new IGatewayClient Client { get; }

    IClient IClientEntity.Client => Client;
}