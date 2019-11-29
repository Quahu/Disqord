using System.Collections.Generic;
using System.IO;
using Disqord.Models;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class CreateGuildContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("region", NullValueHandling.Ignore)]
        public string Region { get; set; }

        [JsonProperty("icon", NullValueHandling.Ignore)]
        public Stream Icon { get; set; }

        [JsonProperty("verification_level")]
        public VerificationLevel VerificationLevel { get; set; }

        [JsonProperty("default_message_notifications")]
        public DefaultNotificationLevel DefaultNotificationLevel { get; set; }

        [JsonProperty("explicit_content_filter")]
        public ContentFilterLevel ContentFilterLevel { get; set; }

        [JsonProperty("roles", NullValueHandling.Ignore)]
        public IReadOnlyList<RoleModel> Roles { get; set; }

        [JsonProperty("channels", NullValueHandling.Ignore)]
        public IReadOnlyList<ChannelModel> Channels { get; set; }
    }
}
