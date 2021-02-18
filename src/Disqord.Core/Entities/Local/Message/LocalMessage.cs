using System.Collections.Generic;
using Disqord.Collections;

namespace Disqord
{
    public sealed class LocalMessage
    {
        public string Content { get; }

        public bool IsTextToSpeech { get; }

        public LocalEmbed Embed { get; }

        public IReadOnlyList<LocalAttachment> Attachments { get; }

        public LocalMentions Mentions { get; }

        public string Nonce { get; }

        internal LocalMessage(LocalMessageBuilder builder)
        {
            Content = builder.Content;
            IsTextToSpeech = builder.IsTextToSpeech;
            Embed = builder.Embed?.Build();
            Attachments = builder.Attachments.ToReadOnlyList();
            Mentions = builder.Mentions?.Build();
            Nonce = builder.Nonce;
        }
    }
}
