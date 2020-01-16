using System;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface IGuildEmoji : ISnowflakeEntity, ICustomEmoji, IDeletable
    {
        Task ModifyAsync(Action<ModifyGuildEmojiProperties> action, RestRequestOptions options = null);
    }
}
