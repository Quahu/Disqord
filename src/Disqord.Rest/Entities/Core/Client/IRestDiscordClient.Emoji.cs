using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IRestDiscordClient : IDisposable
    {
        Task<IReadOnlyList<RestGuildEmoji>> GetGuildEmojisAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestGuildEmoji> GetGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null);

        Task<RestGuildEmoji> CreateGuildEmojiAsync(Snowflake guildId, LocalAttachment image, string name = null, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null);

        Task<RestGuildEmoji> ModifyGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null);

        Task DeleteGuildEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null);
    }
}
