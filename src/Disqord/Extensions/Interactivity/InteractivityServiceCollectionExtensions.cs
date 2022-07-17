using System.ComponentModel;
using Disqord.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Extensions.Interactivity;

[EditorBrowsable(EditorBrowsableState.Never)]
public static class InteractivityServiceCollectionExtensions
{
    public static IServiceCollection AddInteractivityExtension(this IServiceCollection services)
    {
        services.TryAddSingletonEnumerable<DiscordClientExtension, InteractivityExtension>();
        return services;
    }
}