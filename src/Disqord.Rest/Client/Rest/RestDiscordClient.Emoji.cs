using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<IReadOnlyList<RestGuildEmoji>> GetGuildEmojisAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.ListGuildEmojisAsync(guildId, options).ConfigureAwait(false);
            return models.ToReadOnlyList((this, guildId), (x, tuple) =>
            {
                var (@this, guildId) = tuple;
                return new RestGuildEmoji(@this, guildId, x);
            });
        }

        public async Task<RestGuildEmoji> GetGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetGuildEmojiAsync(guildId, emojiId, options).ConfigureAwait(false);
                return new RestGuildEmoji(this, guildId, model);
            }
            catch (DiscordHttpException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound && ex.JsonErrorCode == JsonErrorCode.UnknownEmoji)
            {
                return null;
            }
        }

        public async Task<RestGuildEmoji> CreateGuildEmojiAsync(Snowflake guildId, Stream image, string name, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var model = await ApiClient.CreateGuildEmojiAsync(guildId, image, name, roleIds?.Select(x => x.RawValue), options).ConfigureAwait(false);
            return new RestGuildEmoji(this, guildId, model);
        }

        public async Task<RestGuildEmoji> ModifyGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            var model = await InternalModifyGuildEmojiAsync(guildId, emojiId, action, options).ConfigureAwait(false);
            return new RestGuildEmoji(this, guildId, model);
        }

        internal async Task<EmojiModel> InternalModifyGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyGuildEmojiProperties();
            action(properties);
            var model = await ApiClient.ModifyGuildEmojiAsync(guildId, emojiId, properties, options).ConfigureAwait(false);
            return model;
        }

        public Task DeleteGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null)
            => ApiClient.DeleteGuildEmojiAsync(guildId, emojiId, options);
    }
}
