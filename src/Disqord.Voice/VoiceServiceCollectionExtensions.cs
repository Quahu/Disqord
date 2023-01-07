using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using Disqord.DependencyInjection.Extensions;
using Disqord.Voice.Api;
using Disqord.Voice.Api.Default;
using Disqord.Voice.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Voice;

public static class VoiceServiceCollectionExtensions
{
    public static IServiceCollection AddVoice(this IServiceCollection services)
    {
        try
        {
            RuntimeHelpers.RunClassConstructor(typeof(Sodium).TypeHandle);
        }
        catch (TypeInitializationException ex)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
        }

        services.AddJsonSerializer();

        services.AddWebSocketClientFactory();
        services.TryAddSingleton<IVoiceGatewayClientFactory, DefaultVoiceGatewayClientFactory>();

        services.AddUdpClientFactory();
        services.TryAddSingleton<IVoiceUdpClientFactory, DefaultVoiceUdpClientFactory>();

        services.TryAddSingleton<IVoiceEncryptionProvider, DefaultVoiceEncryptionProvider>();
        services.TryAddSingleton<IVoiceConnectionFactory, DefaultVoiceConnectionFactory>();

        if (OperatingSystem.IsWindows())
        {
            services.AddVoiceSynchronizer<MultimediaTimerVoiceSynchronizer>();
        }
        else
        {
            services.AddVoiceSynchronizer<ThreadPoolTimerVoiceSynchronizer>();
        }

        return services;
    }

    public static IServiceCollection AddVoiceSynchronizer<TSynchronizer>(this IServiceCollection services)
        where TSynchronizer : class, IVoiceSynchronizer
    {
        services.TryAddSingleton<IVoiceSynchronizer, TSynchronizer>();
        return services;
    }
}
