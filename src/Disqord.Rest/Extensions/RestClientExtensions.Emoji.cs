using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Rest.Api;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IGuildEmoji>> FetchGuildEmojisAsync(this IRestClient client, Snowflake guildId, IRestRequestOptions options = null)
        {
            var models = await client.ApiClient.FetchGuildEmojisAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static(x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientGuildEmoji(client, guildId, x);
            });
        }

        public static async Task<IGuildEmoji> FetchGuildEmojiAsync(this IRestClient client, Snowflake guildId, Snowflake emojiId, IRestRequestOptions options = null)
        {
            var model = await client.ApiClient.FetchGuildEmojiAsync(guildId, emojiId, options).ConfigureAwait(false);
            return new TransientGuildEmoji(client, guildId, model);
        }

        public static async Task<IGuildEmoji> CreateGuildEmojiAsync(this IRestClient client, Snowflake guildId, string name, Stream image, Action<CreateGuildEmojiActionProperties> action = null, IRestRequestOptions options = null)
        {
            var properties = new CreateGuildEmojiActionProperties();
            action?.Invoke(properties);
            var content = new CreateGuildEmojiJsonRestRequestContent(name, image)
            {
                Roles = Optional.Convert(properties.RoleIds, x => x.ToArray())
            };
            var model = await client.ApiClient.CreateGuildEmojiAsync(guildId, content, options).ConfigureAwait(false);
            return new TransientGuildEmoji(client, guildId, model);
        }

        public static async Task<IGuildEmoji> ModifyGuildEmojiAsync(this IRestClient client, Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiActionProperties> action = null, IRestRequestOptions options = null)
        {
            var properties = new ModifyGuildEmojiActionProperties();
            action?.Invoke(properties);
            var content = new ModifyGuildEmojiJsonRestRequestContent
            {
                Name = properties.Name,
                Roles = Optional.Convert(properties.RoleIds, x => x.ToArray())
            };
            var model = await client.ApiClient.ModifyGuildEmojiAsync(guildId, emojiId, content, options).ConfigureAwait(false);
            return new TransientGuildEmoji(client, guildId, model);
        }

        public static Task DeleteGuildEmojiAsync(this IRestClient client, Snowflake guildId, Snowflake emojiId, IRestRequestOptions options = null)
            => client.ApiClient.DeleteGuildEmojiAsync(guildId, emojiId, options);
    }
}
