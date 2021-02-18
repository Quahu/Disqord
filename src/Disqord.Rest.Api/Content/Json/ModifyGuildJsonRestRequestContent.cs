﻿using System.IO;
using Disqord.Serialization.Json;

namespace Disqord.Rest.Api
{
    public class ModifyGuildJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public Optional<string> Name;

        [JsonProperty("region")]
        public Optional<string> Region;

        [JsonProperty("verification_level")]
        public Optional<GuildVerificationLevel> VerificationLevel;

        [JsonProperty("default_message_notifications")]
        public Optional<GuildNotificationLevel> DefaultMessageNotifications;

        [JsonProperty("explicit_content_filter")]
        public Optional<GuildContentFilterLevel> ExplicitContentFilter;

        [JsonProperty("afk_channel_id")]
        public Optional<Snowflake?> AfkChannelId;

        [JsonProperty("afk_timeout")]
        public Optional<int> AfkTimeout;

        [JsonProperty("icon")]
        public Optional<Stream> Icon;

        [JsonProperty("owner_id")]
        public Optional<Snowflake> OwnerId;

        [JsonProperty("splash")]
        public Optional<Stream> Splash;

        [JsonProperty("banner")]
        public Optional<Stream> Banner;

        [JsonProperty("system_channel_id")]
        public Optional<Snowflake?> SystemChannelId;

        [JsonProperty("rules_channel_id")]
        public Optional<Snowflake?> RulesChannelId;

        [JsonProperty("public_updates_channel_id")]
        public Optional<Snowflake?> PublicUpdatesChannelId;

        [JsonProperty("preferred_locale")]
        public Optional<string> PreferredLocale;
    }
}
