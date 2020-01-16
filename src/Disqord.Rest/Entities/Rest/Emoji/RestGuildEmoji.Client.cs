using System;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public sealed partial class RestGuildEmoji : RestSnowflakeEntity, IGuildEmoji
    {
        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteGuildEmojiAsync(GuildId, Id, options);

        public async Task ModifyAsync(Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.InternalModifyGuildEmojiAsync(GuildId, Id, action, options).ConfigureAwait(false);
            Update(model);
        }
    }
}