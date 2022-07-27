using Disqord.Logging;
using Qommon.Binding;

namespace Disqord.Gateway.Api;

public interface IShardFactory : IBindable<IGatewayApiClient>, ILogging
{
    IGatewayApiClient ApiClient { get; }

    IShard Create(ShardId id);
}
