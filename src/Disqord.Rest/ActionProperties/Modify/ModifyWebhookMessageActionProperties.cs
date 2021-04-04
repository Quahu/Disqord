using System.Collections.Generic;

namespace Disqord
{
    public sealed class ModifyWebhookMessageActionProperties
    {
        public Optional<string> Content { internal get; set; }

        public Optional<IEnumerable<LocalEmbed>> Embeds { internal get; set; }

        public Optional<LocalAttachment> Attachment { internal get; set; }

        public Optional<LocalMentions> Mentions { internal get; set; }

        internal ModifyWebhookMessageActionProperties()
        { }
    }
}
