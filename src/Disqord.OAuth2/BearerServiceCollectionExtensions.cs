using Disqord.OAuth2.Default;
using Disqord.Rest;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Disqord.OAuth2;

public static class WebhookServiceCollectionExtensions
{
    /// <summary>
    ///     Adds <see cref="DefaultBearerClientFactory"/> to the services.
    /// </summary>
    /// <param name="services"> The services to add to. </param>
    /// <returns>
    ///     The passed services.
    /// </returns>
    public static IServiceCollection AddBearerClientFactory(this IServiceCollection services)
    {
        services.AddToken(Token.None);
        services.AddRestClient();
        services.TryAddSingleton<IBearerClientFactory, DefaultBearerClientFactory>();

        return services;
    }
}