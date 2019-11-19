using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Disqord.Models;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IRestDiscordClient
    {
        public async Task<IReadOnlyList<RestGuildEmoji>> GetGuildEmojisAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.ListGuildEmojisAsync(guildId, options).ConfigureAwait(false);
            return models.Select(x => new RestGuildEmoji(this, guildId, x)).ToImmutableArray();
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

        public async Task<RestGuildEmoji> CreateGuildEmojiAsync(Snowflake guildId, LocalAttachment image, string name = null, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            name = name ?? Path.GetFileNameWithoutExtension(image.FileName);
            var model = await ApiClient.CreateGuildEmojiAsync(guildId, name, image, roleIds?.Select(x => x.RawValue), options).ConfigureAwait(false);
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
