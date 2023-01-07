using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Api.Default;

public class DefaultVoiceGatewayClientFactory : IVoiceGatewayClientFactory
{
    private readonly IServiceProvider _services;

    public DefaultVoiceGatewayClientFactory(IServiceProvider services)
    {
        _services = services;
    }

    protected virtual IVoiceGatewayHeartbeater CreateHeartbeater()
    {
        return Unsafe.As<IVoiceGatewayHeartbeater>(HeartbeaterFactory(_services, null));
    }

    protected virtual IVoiceGateway CreateGateway()
    {
        return Unsafe.As<IVoiceGateway>(GatewayFactory(_services, null));
    }

    protected virtual IVoiceGatewayClient CreateClient(Snowflake guildId, Snowflake currentMemberId,
        string sessionId, string token, string endpoint,
        ILogger logger, IVoiceGatewayHeartbeater heartbeater, IVoiceGateway gateway)
    {
        var apiClient = ClientFactory(_services, new object?[] { guildId, currentMemberId, sessionId, token, endpoint, logger, heartbeater, gateway });
        return Unsafe.As<IVoiceGatewayClient>(apiClient);
    }

    public virtual IVoiceGatewayClient Create(Snowflake guildId, Snowflake currentMemberId, string sessionId, string token, string endpoint, ILogger logger)
    {
        var heartbeater = CreateHeartbeater();
        var gateway = CreateGateway();
        var shard = CreateClient(guildId, currentMemberId, sessionId, token, endpoint, logger, heartbeater, gateway);
        return shard;
    }

    protected static readonly ObjectFactory HeartbeaterFactory;
    protected static readonly ObjectFactory GatewayFactory;
    protected static readonly ObjectFactory ClientFactory;

    static DefaultVoiceGatewayClientFactory()
    {
        HeartbeaterFactory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceGatewayHeartbeater),
            Array.Empty<Type>());

        GatewayFactory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceGateway),
            Array.Empty<Type>());

        ClientFactory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceGatewayClient),
            new[] { typeof(Snowflake), typeof(Snowflake),
                typeof(string), typeof(string), typeof(string),
                typeof(ILogger), typeof(IVoiceGatewayHeartbeater), typeof(IVoiceGateway) });
    }
}
