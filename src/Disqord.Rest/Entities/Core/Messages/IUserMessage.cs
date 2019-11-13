using System;
using System.Collections.Generic;

namespace Disqord
{
    public partial interface IUserMessage : IMessage
    {
        DateTimeOffset? EditedAt { get; }

        Snowflake? WebhookId { get; }

        bool IsTextToSpeech { get; }

        string Nonce { get; }

        bool IsPinned { get; }

        bool MentionsEveryone { get; }

        IReadOnlyList<Snowflake> MentionedRoleIds { get; }

        IReadOnlyList<Attachment> Attachments { get; }

        IReadOnlyList<Embed> Embeds { get; }

        MessageFlags Flags { get; }

        MessageActivity Activity { get; }

        MessageApplication Application { get; }

        MessageReference Reference { get; }
    }
}
