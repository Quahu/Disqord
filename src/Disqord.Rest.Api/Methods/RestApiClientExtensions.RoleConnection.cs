using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<ApplicationRoleConnectionMetadataJsonModel[]> FetchApplicationRoleConnectionMetadataAsync(this IRestApiClient client,
        Snowflake applicationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.RoleConnection.GetMetadata, applicationId);
        return client.ExecuteAsync<ApplicationRoleConnectionMetadataJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<ApplicationRoleConnectionMetadataJsonModel[]> SetApplicationRoleConnectionMetadataAsync(this IRestApiClient client,
        Snowflake applicationId,
        JsonObjectRestRequestContent<ApplicationRoleConnectionMetadataJsonModel[]> content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.RoleConnection.SetMetadata, applicationId);
        return client.ExecuteAsync<ApplicationRoleConnectionMetadataJsonModel[]>(route, content, options, cancellationToken);
    }
}
