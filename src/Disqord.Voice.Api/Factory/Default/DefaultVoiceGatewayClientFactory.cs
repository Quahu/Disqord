using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Api.Default;

public class DefaultVoiceGatewayClientFactory(IServiceProvider services) : IVoiceGatewayClientFactory
{
    protected virtual IVoiceGatewayHeartbeater CreateHeartbeater()
    {
        return Unsafe.As<IVoiceGatewayHeartbeater>(HeartbeaterFactory(services, null));
    }

    protected virtual IVoiceGateway CreateGateway()
    {
        return Unsafe.As<IVoiceGateway>(GatewayFactory(services, null));
    }

    protected virtual IVoiceGatewayClient CreateClient(Snowflake guildId, Snowflake currentMemberId,
        string sessionId, string token, string endpoint, int maxDaveProtocolVersion,
        ILogger logger, IVoiceGatewayHeartbeater heartbeater, IVoiceGateway gateway)
    {
        var apiClient = ClientFactory(services, [guildId, currentMemberId, sessionId, token, endpoint, maxDaveProtocolVersion, logger, heartbeater, gateway]);
        return Unsafe.As<IVoiceGatewayClient>(apiClient);
    }

    public virtual IVoiceGatewayClient Create(Snowflake guildId, Snowflake currentMemberId, string sessionId, string token, string endpoint, int maxDaveProtocolVersion, ILogger logger)
    {
        var heartbeater = CreateHeartbeater();
        var gateway = CreateGateway();
        var shard = CreateClient(guildId, currentMemberId, sessionId, token, endpoint, maxDaveProtocolVersion, logger, heartbeater, gateway);
        return shard;
    }

    protected static readonly ObjectFactory HeartbeaterFactory;
    protected static readonly ObjectFactory GatewayFactory;
    protected static readonly ObjectFactory ClientFactory;

    static DefaultVoiceGatewayClientFactory()
    {
        HeartbeaterFactory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceGatewayHeartbeater), []);

        GatewayFactory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceGateway), []);

        ClientFactory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceGatewayClient),
        [
            typeof(Snowflake), typeof(Snowflake),
            typeof(string), typeof(string), typeof(string),
            typeof(int), typeof(ILogger), typeof(IVoiceGatewayHeartbeater), typeof(IVoiceGateway)
        ]);
    }
}
