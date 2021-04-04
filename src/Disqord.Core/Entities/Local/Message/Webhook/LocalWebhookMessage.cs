using System.Collections.Generic;
using Disqord.Collections;

namespace Disqord
{
    public sealed class LocalWebhookMessage
    {
        public string Content { get; }

        public string Name { get; }

        public string AvatarUrl { get; }

        public bool IsTextToSpeech { get; }

        public LocalAttachment Attachment { get; }

        public IReadOnlyList<LocalEmbed> Embeds { get; }

        public LocalMentions Mentions { get; }

        public LocalWebhookMessage(LocalWebhookMessageBuilder builder)
        {
            Content = builder.Content;
            Name = builder.Name;
            AvatarUrl = builder.AvatarUrl;
            IsTextToSpeech = builder.IsTextToSpeech;
            Attachment = builder.Attachment;
            Embeds = builder.Embeds.ToReadOnlyList(x => x.Build());
            Mentions = builder.Mentions?.Build();
        }
    }
}
