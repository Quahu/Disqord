using System;
using Disqord.Gateway.Api;
using Disqord.Gateway.Api.Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Binding;

namespace Disqord.Sharding;

public class DefaultShardFactory : IShardFactory
{
    public ILogger Logger { get; }

    public DiscordClientSharder Sharder => _binder.Value;

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
        options = Options.Create(value);
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger($"Shard #{id.Id}");
        var shard = _factory(services, new object[] { options, logger });
        return (shard as IGatewayApiClient)!;
    }

    private static readonly ObjectFactory _factory;

    static DefaultShardFactory()
    {
        _factory = ActivatorUtilities.CreateFactory(typeof(DefaultGatewayApiClient), new[] { typeof(IOptions<DefaultGatewayApiClientConfiguration>), typeof(ILogger) });
    }
}
