using System;
using Disqord.Gateway.Api;
using Disqord.Logging;
using Disqord.Utilities.Binding;

namespace Disqord.Sharding
{
    public interface IShardFactory : IBindable<DiscordClientSharder>, ILogging
    {
        DiscordClientSharder Sharder { get; }

        IGatewayApiClient Create(ShardId id, IServiceProvider services);
    }
}
