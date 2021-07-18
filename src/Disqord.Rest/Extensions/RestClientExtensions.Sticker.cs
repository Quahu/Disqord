using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<ISticker> FetchStickerAsync(this IRestClient client, Snowflake stickerId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchStickerAsync(stickerId, options).ConfigureAwait(false);
            return TransientSticker.Create(client, model);
        }

        public static async Task<IReadOnlyList<IGuildSticker>> FetchGuildStickersAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildStickersAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList(client, static(x, client) => new TransientGuildSticker(client, x));
        }

        public static async Task<IGuildSticker> FetchGuildStickerAsync(this IRestClient client, Snowflake guildId, Snowflake stickerId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGuildStickerAsync(guildId, stickerId, options).ConfigureAwait(false);
            return new TransientGuildSticker(client, model);
        }

        public static async Task<IGuildSticker> CreateGuildStickerAsync(this IRestClient client, Snowflake guildId, string name, string tags, Stream file, Action<CreateGuildStickerActionProperties> action = null, IRestRequestOptions options = null)
        {
            var properties = new CreateGuildStickerActionProperties();
            action?.Invoke(properties);

            var content = new CreateGuildStickerJsonRestRequestContent
            {
                Name = name,
                Description = properties.Description,
                Tags = tags,
                File = file
            };

            var model = await client.ApiClient.CreateGuildStickerAsync(guildId, content, options).ConfigureAwait(false);
            return new TransientGuildSticker(client, model);
        }

        public static async Task<IGuildSticker> ModifyGuildStickerAsync(this IRestClient client, Snowflake guildId, Snowflake stickerId, Action<ModifyGuildStickerActionProperties> action, IRestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyGuildStickerActionProperties();
            action(properties);

            var content = new ModifyGuildStickerJsonRestRequestContent
            {
                Name = properties.Name,
                Description = properties.Description,
                Tags = properties.Tags
            };

            var model = await client.ApiClient.ModifyGuildStickerAsync(guildId, stickerId, content, options).ConfigureAwait(false);
            return new TransientGuildSticker(client, model);
        }

        public static Task DeleteGuildStickerAsync(this IRestClient client, Snowflake guildId, Snowflake stickerId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteGuildStickerAsync(guildId, stickerId, options);
    }
}