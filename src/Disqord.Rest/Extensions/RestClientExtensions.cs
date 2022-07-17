using System.ComponentModel;

namespace Disqord.Rest;

[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class RestClientExtensions
{
    //private static IRestApiClient GetApiClient(IDiscordEntity entity)
    //{
    //    if (entity.Client is not IRestDiscordClient client)
    //        throw new NotSupportedException("This entity's client does not support executing REST requests.");

    //    return client.ApiClient;
    //}
}