using System;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Default;
using Disqord.Utilities.Binding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Sharding
{
    public class DefaultShardFactory : IShardFactory
    {
        public ILogger Logger { get; }

        public DiscordClientSharder Sharder { get; }

        private readonly Binder<DiscordClientSharder> _binder;

        public DefaultShardFactory(
            ILogger<DefaultShardFactory> logger)
        {
            Logger = logger;

            _binder = new(this);
        }

        public void Bind(DiscordClientSharder value)
        {
            _binder.Bind(value);
        }

        public IGatewayApiClient Create(ShardId id, IServiceProvider services)
        {
            var options = services.GetRequiredService<IOptions<DefaultGatewayApiClientConfiguration>>();
            var value = options.Value.Clone();
            value.Id = id;
            // TODO: presence
            options = Options.Create(value);
            var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger($"Shard #{id.Id}");
            var shard = _factory(services, new object[] { options, logger });
            return shard as IGatewayApiClient;
        }

        private static readonly ObjectFactory _factory;

        static DefaultShardFactory()
        {
            _factory = ActivatorUtilities.CreateFactory(typeof(DefaultGatewayApiClient), new[] { typeof(IOptions<DefaultGatewayApiClientConfiguration>), typeof(ILogger) });
        }
    }
}
