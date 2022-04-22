using System.IO;
using Qommon;

namespace Disqord
{
    public sealed class ModifyWebhookActionProperties
    {
        public Optional<string> Name { internal get; set; }

        public Optional<Stream> Avatar { internal get; set; }

        public Optional<Snowflake> ChannelId { internal get; set; }

        internal ModifyWebhookActionProperties()
        { }

        internal bool HasValues
            => Name.HasValue || Avatar.HasValue || ChannelId.HasValue;
    }
}
