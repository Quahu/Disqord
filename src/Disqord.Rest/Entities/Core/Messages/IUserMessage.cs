using System;
using System.Collections.Generic;

namespace Disqord
{
    public partial interface IUserMessage : IMessage
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
    }
}
