using System.Collections.Generic;

namespace Disqord
{
    public sealed class ModifyMessageActionProperties
    {
        public Optional<string> Content { internal get; set; }

        public Optional<IEnumerable<LocalEmbed>> Embeds { internal get; set; }

        public Optional<MessageFlag> Flags { internal get; set; }

        public Optional<LocalAllowedMentions> AllowedMentions { internal get; set; }

        public Optional<LocalAttachment> Attachments { internal get; set; }

        public Optional<IEnumerable<Snowflake>> AttachmentIds { internal get; set; }

        public Optional<IEnumerable<LocalRowComponent>> Components { internal get; set; }

        public Optional<IEnumerable<Snowflake>> StickerIds { internal get; set; }

        internal ModifyMessageActionProperties()
        { }
    }
}
