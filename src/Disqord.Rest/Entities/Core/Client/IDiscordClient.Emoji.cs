using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IDiscordClient : IDisposable
    {
        Task<IReadOnlyList<RestGuildEmoji>> GetEmojisAsync(Snowflake guildId, RestRequestOptions options = null);

        Task<RestGuildEmoji> GetEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null);

        Task<RestGuildEmoji> CreateEmojiAsync(Snowflake guildId, string name, LocalAttachment image, IEnumerable<Snowflake> roleIds = null, RestRequestOptions options = null);

        Task<RestGuildEmoji> ModifyEmojiAsync(Snowflake guildId, Snowflake emojiId, Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null);

        Task DeleteEmojiAsync(Snowflake guildId, Snowflake emojiId, RestRequestOptions options = null);
    }
}
