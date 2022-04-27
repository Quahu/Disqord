using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Api;
using Qommon;
using Qommon.Collections;
using Qommon.Collections.ReadOnly;

namespace Disqord.Rest
{
    public static partial class RestClientExtensions
    {
        public static async Task<IReadOnlyList<IGuildEmoji>> FetchGuildEmojisAsync(this IRestClient client,
            Snowflake guildId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var models = await client.ApiClient.FetchGuildEmojisAsync(guildId, options, cancellationToken).ConfigureAwait(false);
            return models.ToReadOnlyList((client, guildId), static (x, tuple) =>
            {
                var (client, guildId) = tuple;
                return new TransientGuildEmoji(client, guildId, x);
            });
        }

        public static async Task<IGuildEmoji> FetchGuildEmojiAsync(this IRestClient client,
            Snowflake guildId, Snowflake emojiId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var model = await client.ApiClient.FetchGuildEmojiAsync(guildId, emojiId, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildEmoji(client, guildId, model);
        }

        public static async Task<IGuildEmoji> CreateGuildEmojiAsync(this IRestClient client,
            Snowflake guildId, string name, Stream image, Action<CreateGuildEmojiActionProperties> action = null,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new CreateGuildEmojiActionProperties();
            action?.Invoke(properties);
            var content = new CreateGuildEmojiJsonRestRequestContent(name, image)
            {
                Roles = Optional.Convert(properties.RoleIds, x => x.ToArray())
            };

            var model = await client.ApiClient.CreateGuildEmojiAsync(guildId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildEmoji(client, guildId, model);
        }

        public static async Task<IGuildEmoji> ModifyGuildEmojiAsync(this IRestClient client,
            Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiActionProperties> action,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            var properties = new ModifyGuildEmojiActionProperties();
            action?.Invoke(properties);
            var content = new ModifyGuildEmojiJsonRestRequestContent
            {
                Name = properties.Name,
                Roles = Optional.Convert(properties.RoleIds, x => x.ToArray())
            };

            var model = await client.ApiClient.ModifyGuildEmojiAsync(guildId, emojiId, content, options, cancellationToken).ConfigureAwait(false);
            return new TransientGuildEmoji(client, guildId, model);
        }

        public static Task DeleteGuildEmojiAsync(this IRestClient client,
            Snowflake guildId, Snowflake emojiId,
            IRestRequestOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.ApiClient.DeleteGuildEmojiAsync(guildId, emojiId, options, cancellationToken);
        }
    }
}
