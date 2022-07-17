using System.Threading;
using System.Threading.Tasks;
using Qommon;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task DeleteAsync(this IIntegration integration,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        // Fix for integrations coming from user connections not having the guild ID.
        Guard.IsNotDefault(integration.GuildId);

        var client = integration.GetRestClient();
        return client.DeleteIntegrationAsync(integration.GuildId, integration.Id, options, cancellationToken);
    }
}