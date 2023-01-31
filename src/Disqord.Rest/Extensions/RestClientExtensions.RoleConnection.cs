using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest;

public static partial class RestClientExtensions
{
    public static async Task<IReadOnlyList<IApplicationRoleConnectionMetadata>> FetchApplicationRoleConnectionMetadataAsync(this IRestClient client,
        Snowflake applicationId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = await client.ApiClient.FetchApplicationRoleConnectionMetadataAsync(applicationId, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(model => new TransientApplicationRoleConnectionMetadata(model));
    }

    public static async Task<IReadOnlyList<IApplicationRoleConnectionMetadata>> SetApplicationRoleConnectionMetadataAsync(this IRestClient client,
        Snowflake applicationId,
        IEnumerable<LocalApplicationRoleConnectionMetadata> metadata,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var models = metadata.Select(metadata => metadata.ToModel()).ToArray();
        models = await client.ApiClient.SetApplicationRoleConnectionMetadataAsync(applicationId, models, options, cancellationToken).ConfigureAwait(false);
        return models.ToReadOnlyList(model => new TransientApplicationRoleConnectionMetadata(model));
    }
}
