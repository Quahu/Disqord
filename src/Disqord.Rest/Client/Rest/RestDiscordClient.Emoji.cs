using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial class RestDiscordClient : IDiscordClient
    {
        public async Task<IReadOnlyList<RestGuildEmoji>> GetEmojisAsync(Snowflake guildId, RestRequestOptions options = null)
        {
            var models = await ApiClient.ListGuildEmojisAsync(guildId, options).ConfigureAwait(false);
            return models.Select(x => new RestGuildEmoji(this, x, guildId)).ToImmutableArray();
        }

        public async Task<RestGuildEmoji> GetEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null)
        {
            try
            {
                var model = await ApiClient.GetGuildEmojiAsync(guildId, emojiId, options).ConfigureAwait(false);
                return new RestGuildEmoji(this, model, guildId);
            }
            catch (HttpDiscordException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound && ex.JsonErrorCode == JsonErrorCode.UnknownEmoji)
            {
                return null;
            }
        }

        public async Task<RestGuildEmoji> CreateEmojiAsync(Snowflake guildId, string name, LocalAttachment image, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null)
        {
            if (image == null)
                throw new ArgumentNullException(nameof(image));

            name = name ?? Path.GetFileNameWithoutExtension(image.FileName);
            var model = await ApiClient.CreateGuildEmojiAsync(guildId, name, image, roleIds.Select(x => x.RawValue), options).ConfigureAwait(false);
            return new RestGuildEmoji(this, model, guildId);
        }

        public async Task<RestGuildEmoji> ModifyEmojiAsync(Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            var properties = new ModifyGuildEmojiProperties();
            action(properties);
            var model = await ApiClient.ModifyGuildEmojiAsync(guildId, emojiId, properties, options).ConfigureAwait(false);
            return new RestGuildEmoji(this, model, guildId);
        }

        public Task DeleteEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null)
            => ApiClient.DeleteGuildEmojiAsync(guildId, emojiId, options);
    }
}
