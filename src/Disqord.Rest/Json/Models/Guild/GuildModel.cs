using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal class GuildModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("name")]
        public Optional<string> Name { get; set; }

        [JsonProperty("icon")]
        public Optional<string> Icon { get; set; }

        [JsonProperty("splash")]
        public Optional<string> Splash { get; set; }

        [JsonProperty("discovery_splash")]
        public Optional<string> DiscoverySplash { get; set; }

        [JsonProperty("owner")]
        public Optional<bool> Owner { get; set; }

        [JsonProperty("owner_id")]
        public Optional<ulong> OwnerId { get; set; }

        [JsonProperty("permissions")]
        public Optional<ulong> Permissions { get; set; }

        [JsonProperty("region")]
        public Optional<string> Region { get; set; }

        [JsonProperty("afk_channel_id")]
        public Optional<ulong?> AfkChannelId { get; set; }

        [JsonProperty("afk_timeout")]
        public Optional<int> AfkTimeout { get; set; }

        [JsonProperty("embed_enabled")]
        public Optional<bool> EmbedEnabled { get; set; }

        [JsonProperty("embed_channel_id")]
        public Optional<ulong?> EmbedChannelId { get; set; }

        [JsonProperty("verification_level")]
        public Optional<VerificationLevel> VerificationLevel { get; set; }

        [JsonProperty("default_message_notifications")]
        public Optional<DefaultNotificationLevel> DefaultMessageNotifications { get; set; }

        [JsonProperty("explicit_content_filter")]
        public Optional<ContentFilterLevel> ExplicitContentFilter { get; set; }

        [JsonProperty("roles")]
        public Optional<RoleModel[]> Roles { get; set; }

        [JsonProperty("emojis")]
        public Optional<EmojiModel[]> Emojis { get; set; }

        [JsonProperty("features")]
        public Optional<string[]> Features { get; set; }

        [JsonProperty("mfa_level")]
        public Optional<MfaLevel> MfaLevel { get; set; }

        [JsonProperty("application_id")]
        public Optional<ulong?> ApplicationId { get; set; }

        [JsonProperty("widget_enabled")]
        public Optional<bool> WidgetEnabled { get; set; }

        [JsonProperty("widget_channel_id")]
        public Optional<ulong?> WidgetChannelId { get; set; }

        [JsonProperty("system_channel_id")]
        public Optional<ulong?> SystemChannelId { get; set; }

        [JsonProperty("max_presences")]
        public Optional<int?> MaxPresences { get; set; }

        [JsonProperty("max_members")]
        public Optional<int> MaxMembers { get; set; }

        [JsonProperty("vanity_url_code")]
        public Optional<string> VanityUrlCode { get; set; }

        [JsonProperty("description")]
        public Optional<string> Description { get; set; }

        [JsonProperty("banner")]
        public Optional<string> Banner { get; set; }

        [JsonProperty("premium_tier")]
        public Optional<BoostTier> PremiumTier { get; set; }

        [JsonProperty("premium_subscription_count")]
        public Optional<int?> PremiumSubscriptionCount { get; set; }

        [JsonProperty("preferred_locale")]
        public Optional<string> PreferredLocale { get; set; }
    }
}
