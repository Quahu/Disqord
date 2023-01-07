using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Voice.Default;

public class DefaultVoiceConnectionFactory : IVoiceConnectionFactory
{
    private readonly IServiceProvider _services;

    public DefaultVoiceConnectionFactory(IServiceProvider services)
    {
        _services = services;
    }

    public IVoiceConnection Create(Snowflake guildId, Snowflake channelId, Snowflake currentMemberId, SetVoiceStateDelegate setVoiceStateDelegate)
    {
        var connection = Factory(_services, new object[] { guildId, channelId, currentMemberId, setVoiceStateDelegate });
        return Unsafe.As<IVoiceConnection>(connection);
    }

    private static readonly ObjectFactory Factory;

    static DefaultVoiceConnectionFactory()
    {
        Factory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceConnection),
            new[] { typeof(Snowflake), typeof(Snowflake), typeof(Snowflake), typeof(SetVoiceStateDelegate) });
    }
}
