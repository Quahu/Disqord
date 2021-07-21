using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest.Api
{
    public static partial class RestApiClientExtensions
    {
        public static Task<StickerJsonModel> FetchStickerAsync(this IRestApiClient client, Snowflake stickerId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Sticker.GetSticker, stickerId);
            return client.ExecuteAsync<StickerJsonModel>(route, null, options);
        }

        public static Task<StickerJsonModel[]> FetchGuildStickersAsync(this IRestApiClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Sticker.GetGuildStickers, guildId);
            return client.ExecuteAsync<StickerJsonModel[]>(route, null, options);
        }

        public static Task<StickerJsonModel> FetchGuildStickerAsync(this IRestApiClient client, Snowflake guildId, Snowflake stickerId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Sticker.GetGuildSticker, guildId, stickerId);
            return client.ExecuteAsync<StickerJsonModel>(route, null, options);
        }

        public static Task<StickerJsonModel> CreateGuildStickerAsync(this IRestApiClient client, Snowflake guildId, CreateGuildStickerJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Sticker.CreateGuildSticker, guildId);
            return client.ExecuteAsync<StickerJsonModel>(route, content, options);
        }

        public static Task<StickerJsonModel> ModifyGuildStickerAsync(this IRestApiClient client, Snowflake guildId, Snowflake stickerId, ModifyGuildStickerJsonRestRequestContent content, IRestRequestOptions options = null)
        {
            var route = Format(Route.Sticker.ModifyGuildSticker, guildId);
            return client.ExecuteAsync<StickerJsonModel>(route, content, options);
        }

        public static Task DeleteGuildStickerAsync(this IRestApiClient client, Snowflake guildId, Snowflake stickerId, IRestRequestOptions options = null)
        {
            var route = Format(Route.Sticker.DeleteGuildSticker, guildId, stickerId);
            return client.ExecuteAsync(route, null, options);
        }
    }
}