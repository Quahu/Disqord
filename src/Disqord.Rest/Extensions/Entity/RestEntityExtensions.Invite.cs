using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task DeleteAsync(this IInvite invite,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = invite.GetRestClient();
        return client.DeleteInviteAsync(invite.Code, options, cancellationToken);
    }
}