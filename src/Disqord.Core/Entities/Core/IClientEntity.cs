namespace Disqord;

/// <summary>
///     Represents a Discord entity tied to a client.
/// </summary>
public interface IClientEntity : IEntity
{
    /// <summary>
    ///     Gets the client that created this entity.
    /// </summary>
    IClient Client { get; }
}