using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public static partial class RestEntityExtensions
    {
        public static Task<IGuildEmoji> ModifyAsync(this IGuildEmoji emoji, Snowflake emojiId, Action<ModifyGuildEmojiActionProperties> action, IRestRequestOptions options = null)
        {
            var client = emoji.GetRestClient();
            return client.ModifyGuildEmojiAsync(emoji.Id, emojiId, action, options);
        }

        public static Task DeleteAsync(this IGuildEmoji emoji, Snowflake emojiId, IRestRequestOptions options = null)
        {
            var client = emoji.GetRestClient();
            return client.DeleteGuildEmojiAsync(emoji.Id, emojiId, options);
        }
    }
}
