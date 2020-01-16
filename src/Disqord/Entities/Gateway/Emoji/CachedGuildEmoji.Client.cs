using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public sealed partial class CachedGuildEmoji : CachedSnowflakeEntity, IGuildEmoji
    {
        public async Task ModifyAsync(Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null)
        {
            var model = await Client.RestClient.InternalModifyGuildEmojiAsync(Guild.Id, Id, action, options).ConfigureAwait(false);
            Update(model);
        }

        public Task DeleteAsync(RestRequestOptions options = null)
            => Client.DeleteGuildEmojiAsync(Guild.Id, Id, options);
    }
}