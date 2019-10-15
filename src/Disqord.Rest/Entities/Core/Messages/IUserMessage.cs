using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord
{
    public interface IUserMessage : IMessage
    {
        DateTimeOffset? EditedTimestamp { get; }

        Snowflake? WebhookId { get; }

        bool IsTextToSpeech { get; }

        Snowflake? Nonce { get; }

        bool IsPinned { get; }

        bool MentionsEveryone { get; }

        IReadOnlyList<Snowflake> RoleIdsMentioned { get; }

        IReadOnlyList<Attachment> Attachments { get; }

        IReadOnlyList<Embed> Embeds { get; }

        Task ModifyAsync(Action<ModifyMessageProperties> action, RestRequestOptions options = null);
    }
}
