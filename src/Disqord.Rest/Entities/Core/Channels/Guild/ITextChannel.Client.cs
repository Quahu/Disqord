using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Disqord.Rest;

namespace Disqord
{
    public partial interface ITextChannel : INestedChannel, IMessageChannel, IMentionable, ITaggable
    {
        Task ModifyAsync(Action<ModifyTextChannelProperties> action, RestRequestOptions options = null);

        Task<RestWebhook> CreateWebhookAsync(string name, Stream avatar = null, RestRequestOptions options = null);

        Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null);

        RestRequestEnumerator<Snowflake> GetBulkMessageDeletionEnumerator(IEnumerable<Snowflake> messageIds, RestRequestOptions options = null);

        Task DeleteMessagesAsync(IEnumerable<Snowflake> messageIds, RestRequestOptions options = null);

        Task RemoveMemberReactionAsync(Snowflake messageId, Snowflake memberId, IEmoji emoji, RestRequestOptions options = null);

        Task ClearReactionsAsync(Snowflake messageId, IEmoji emoji = null, RestRequestOptions options = null);
    }
}
