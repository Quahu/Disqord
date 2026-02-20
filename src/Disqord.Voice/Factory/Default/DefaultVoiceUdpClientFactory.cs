using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Disqord.Voice.Default;

public class DefaultVoiceUdpClientFactory(IServiceProvider services) : IVoiceUdpClientFactory
{
    public IVoiceUdpClient Create(uint ssrc, string hostName, int port, ILogger logger, IVoiceEncryption encryption)
    {
        var udp = Factory(services, [ssrc, hostName, port, logger, encryption]);
        return Unsafe.As<IVoiceUdpClient>(udp);
    }

    private static readonly ObjectFactory Factory;

    static DefaultVoiceUdpClientFactory()
    {
        Factory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceUdpClient),
            [typeof(uint), typeof(string), typeof(int), typeof(ILogger), typeof(IVoiceEncryption)]);
    }
}
