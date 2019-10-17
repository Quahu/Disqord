using Disqord.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public partial interface ITextChannel : IGuildChannel, IMessageChannel, IMentionable, ITaggable
    {
        Task ModifyAsync(Action<ModifyTextChannelProperties> action, RestRequestOptions options = null);

        Task<RestWebhook> CreateWebhookAsync(string name, LocalAttachment avatar = null, RestRequestOptions options = null);

        Task<IReadOnlyList<RestWebhook>> GetWebhooksAsync(RestRequestOptions options = null);

        RestRequestEnumerator<Snowflake> GetBulkMessageDeletionEnumerator(IEnumerable<Snowflake> messageIds);

        Task DeleteMessagesAsync(IEnumerable<Snowflake> messageIds, RestRequestOptions options = null);

        Task RemoveMemberReactionAsync(Snowflake messageId, Snowflake memberId, IEmoji emoji, RestRequestOptions options = null);

        Task RemoveAllReactionsAsync(Snowflake messageId, RestRequestOptions options = null);
    }
}
