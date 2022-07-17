using System.Linq;
using Disqord.DependencyInjection.Extensions;
using Disqord.Rest;
using Disqord.Webhook.Default;
using Microsoft.Extensions.DependencyInjection;

namespace Disqord.Webhook;

public static class WebhookServiceCollectionExtensions
{
    public static IServiceCollection AddWebhookClientFactory(this IServiceCollection services)
    {
        if (!services.Any(x => x.ServiceType == typeof(IRestClient)))
        {
            // This *should* mean the client is being added by the user, and not the lib.
            // We try to add a dummy token to skip the authorization header for requests.
            services.AddToken(Token.None);
            services.AddRestClient();
        }

        services.TryAddSingleton<IWebhookClientFactory, DefaultWebhookClientFactory>();

        return services;
    }
}