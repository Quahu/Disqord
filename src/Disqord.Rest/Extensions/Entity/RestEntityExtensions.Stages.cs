using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public partial class RestEntityExtensions
{
    public static Task<IStage> ModifyAsync(this IStage instance,
        Action<ModifyStageActionProperties> action,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = instance.GetRestClient();
        return client.ModifyStageAsync(instance.ChannelId, action, options, cancellationToken);
    }

    public static Task DeleteAsync(this IStage instance,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = instance.GetRestClient();
        return client.DeleteStageAsync(instance.ChannelId, options, cancellationToken);
    }
}