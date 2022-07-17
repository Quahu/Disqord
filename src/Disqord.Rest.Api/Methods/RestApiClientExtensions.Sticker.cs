using System.Threading;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api;

public static partial class RestApiClientExtensions
{
    public static Task<StickerJsonModel> FetchStickerAsync(this IRestApiClient client,
        Snowflake stickerId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Sticker.GetSticker, stickerId);
        return client.ExecuteAsync<StickerJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<StickerPackListJsonModel> FetchStickerPacksAsync(this IRestApiClient client,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Sticker.GetStickerPacks);
        return client.ExecuteAsync<StickerPackListJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<StickerJsonModel[]> FetchGuildStickersAsync(this IRestApiClient client,
        Snowflake guildId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Sticker.GetGuildStickers, guildId);
        return client.ExecuteAsync<StickerJsonModel[]>(route, null, options, cancellationToken);
    }

    public static Task<StickerJsonModel> FetchGuildStickerAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake stickerId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Sticker.GetGuildSticker, guildId, stickerId);
        return client.ExecuteAsync<StickerJsonModel>(route, null, options, cancellationToken);
    }

    public static Task<StickerJsonModel> CreateGuildStickerAsync(this IRestApiClient client,
        Snowflake guildId, CreateGuildStickerMultipartRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Sticker.CreateGuildSticker, guildId);
        return client.ExecuteAsync<StickerJsonModel>(route, content, options, cancellationToken);
    }

    public static Task<StickerJsonModel> ModifyGuildStickerAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake stickerId, ModifyGuildStickerJsonRestRequestContent content,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Sticker.ModifyGuildSticker, guildId, stickerId);
        return client.ExecuteAsync<StickerJsonModel>(route, content, options, cancellationToken);
    }

    public static Task DeleteGuildStickerAsync(this IRestApiClient client,
        Snowflake guildId, Snowflake stickerId,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        var route = Format(Route.Sticker.DeleteGuildSticker, guildId, stickerId);
        return client.ExecuteAsync(route, null, options, cancellationToken);
    }
}