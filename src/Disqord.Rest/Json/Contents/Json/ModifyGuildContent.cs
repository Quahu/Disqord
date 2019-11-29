using System.IO;
using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class ModifyGuildContent : JsonRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name { get; set; }

        [JsonProperty("region")]
        public Optional<string> Region { get; set; }

        [JsonProperty("verification_level")]
        public Optional<VerificationLevel> VerificationLevel { get; set; }

        [JsonProperty("default_message_notifications")]
        public Optional<DefaultNotificationLevel> DefaultNotificationLevel { get; set; }

        [JsonProperty("explicit_content_filter")]
        public Optional<ContentFilterLevel> ContentFilterLevel { get; set; }

        [JsonProperty("afk_channel_id")]
        public Optional<ulong> AfkChannelId { get; set; }

        [JsonProperty("afk_timeout")]
        public Optional<int> AfkTimeout { get; set; }

        [JsonProperty("icon")]
        public Optional<Stream> Icon { get; set; }

        [JsonProperty("owner_id")]
        public Optional<ulong> OwnerId { get; set; }

        [JsonProperty("splash")]
        public Optional<Stream> Splash { get; set; }

        [JsonProperty("system_channel_id")]
        public Optional<ulong> SystemChannelId { get; set; }

        [JsonProperty("banner")]
        public Optional<Stream> Banner { get; set; }
    }
}
