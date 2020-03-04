using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<IReadOnlyList<RestGuildEmoji>> GetGuildEmojisAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestGuildEmoji> GetGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null);

        Task<RestGuildEmoji> CreateGuildEmojiAsync(Snowflake guildId, Stream image, string name, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null);

        Task<RestGuildEmoji> ModifyGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null);

        Task DeleteGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null);
    }
}
