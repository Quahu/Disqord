using System.IO;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class CreateGuildJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("icon")]
        public Optional<Stream> Icon;

        [JsonProperty("verification_level")]
        public Optional<GuildVerificationLevel> VerificationLevel;

        [JsonProperty("default_message_notifications")]
        public Optional<GuildNotificationLevel> DefaultMessageNotifications;

        [JsonProperty("explicit_content_filter")]
        public Optional<GuildContentFilterLevel> ExplicitContentFilter;

        [JsonProperty("roles")]
        public Optional<RoleJsonModel> Roles;

        [JsonProperty("channels")]
        public Optional<ChannelJsonModel> Channels;

        [JsonProperty("afk_channel_id")]
        public Optional<Snowflake> AfkChannelId;

        [JsonProperty("afk_timeout")]
        public Optional<int> AfkTimeout;

        [JsonProperty("system_channel_id")]
        public Optional<Snowflake> SystemChannelId;
    }
}
