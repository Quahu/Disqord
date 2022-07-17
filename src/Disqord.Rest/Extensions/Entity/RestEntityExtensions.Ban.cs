using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

public static partial class RestEntityExtensions
{
    public static Task DeleteAsync(this IBan ban,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var client = ban.GetRestClient();
        return client.DeleteBanAsync(ban.GuildId, ban.User.Id, options, cancellationToken);
    }
}