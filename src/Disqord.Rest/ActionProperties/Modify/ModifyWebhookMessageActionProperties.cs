using System.Collections.Generic;
using Qommon;

namespace Disqord
{
    public sealed class ModifyWebhookMessageActionProperties
    {
        public Optional<string> Content { internal get; set; }

        public Optional<IEnumerable<LocalEmbed>> Embeds { internal get; set; }

        public Optional<LocalAllowedMentions> AllowedMentions { internal get; set; }

        public Optional<IEnumerable<LocalAttachment>> Attachments { internal get; set; }

        public Optional<IEnumerable<Snowflake>> AttachmentIds { internal get; set; }

        public Optional<IEnumerable<LocalRowComponent>> Components { internal get; set; }

        internal ModifyWebhookMessageActionProperties()
        { }
    }
}
