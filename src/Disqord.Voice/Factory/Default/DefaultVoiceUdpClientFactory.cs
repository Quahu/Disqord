using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Voice.Default;

public class DefaultVoiceUdpClientFactory : IVoiceUdpClientFactory
{
    private readonly IServiceProvider _services;

    public DefaultVoiceUdpClientFactory(IServiceProvider services)
    {
        _services = services;
    }

    public IVoiceUdpClient Create(uint ssrc, byte[] encryptionKey, string hostName, int port, IVoiceEncryption encryption)
    {
        var udp = Factory(_services, new object[] { ssrc, encryptionKey, hostName, port, encryption });
        return Unsafe.As<IVoiceUdpClient>(udp);
    }

    private static readonly ObjectFactory Factory;

    static DefaultVoiceUdpClientFactory()
    {
        Factory = ActivatorUtilities.CreateFactory(typeof(DefaultVoiceUdpClient),
            new[] { typeof(uint), typeof(byte[]), typeof(string), typeof(int), typeof(IVoiceEncryption) });
    }
}
