using Qommon.Binding;

namespace Disqord.Gateway.Api;

/// <summary>
///     Represents an <see cref="IShard"/> factory.
/// </summary>
public interface IShardFactory : IBindable<IGatewayApiClient>
{
    /// <summary>
    ///     Gets the API client of this factory.
    /// </summary>
    IGatewayApiClient ApiClient { get; }

    /// <summary>
    ///     Creates a shard with the specified ID.
    /// </summary>
    /// <param name="id"> The ID of the shard. </param>
    /// <returns>
    ///     The <see cref="IShard"/> instance.
    /// </returns>
    IShard Create(ShardId id);
}
