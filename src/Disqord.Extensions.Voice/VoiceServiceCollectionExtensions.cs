using System.ComponentModel;
using Disqord.DependencyInjection.Extensions;
using Disqord.Voice;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Extensions.Voice
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class VoiceServiceCollectionExtensions
    {
        public static IServiceCollection AddVoiceExtension(this IServiceCollection services)
        {
            services.AddVoice();
            services.TryAddSingletonEnumerable<DiscordClientExtension, VoiceExtension>();
            return services;
        }
    }
}
