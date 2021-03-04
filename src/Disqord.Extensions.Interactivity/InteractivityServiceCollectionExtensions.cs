using System;
using System.ComponentModel;
using Disqord.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Extensions.Interactivity
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class InteractivityServiceCollectionExtensions
    {
        public static IServiceCollection AddInteractivity(this IServiceCollection services, Action<InteractivityExtensionConfiguration> configure = null)
        {
            if (services.TryAddSingleton<DiscordClientExtension, InteractivityExtension>())
            {
                var options = services.AddOptions<InteractivityExtensionConfiguration>();
                if (configure != null)
                    options.Configure(configure);
            }

            return services;
        }
    }
}
