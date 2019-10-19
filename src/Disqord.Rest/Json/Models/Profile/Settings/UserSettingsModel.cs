using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class UserSettingsModel
    {
        [JsonProperty("timezone_offset")]
        public Optional<int> TimezoneOffset { get; set; }

        [JsonProperty("theme")]
        public Optional<Theme> Theme { get; set; }

        [JsonProperty("stream_notifications_enabled")]
        public Optional<bool> StreamNotificationsEnabled { get; set; }

        [JsonProperty("status")]
        public Optional<UserStatus> Status { get; set; }

        [JsonProperty("show_current_game")]
        public Optional<bool> ShowCurrentGame { get; set; }

        [JsonProperty("restricted_guilds")]
        public Optional<ulong[]> RestrictedGuilds { get; set; }

        [JsonProperty("render_reactions")]
        public Optional<bool> RenderReactions { get; set; }

        [JsonProperty("render_embeds")]
        public Optional<bool> RenderEmbeds { get; set; }

        [JsonProperty("message_display_compact")]
        public Optional<bool> MessageDisplayCompact { get; set; }

        [JsonProperty("locale")]
        public Optional<string> Locale { get; set; }

        [JsonProperty("inline_embed_media")]
        public Optional<bool> InlineEmbedMedia { get; set; }

        [JsonProperty("inline_attachment_media")]
        public Optional<bool> InlineAttachmentMedia { get; set; }

        [JsonProperty("guild_positions")]
        public Optional<ulong[]> GuildPositions { get; set; }

        [JsonProperty("guild_folders")]
        public Optional<GuildFolderModel[]> GuildFolders { get; set; }

        [JsonProperty("gif_auto_play")]
        public Optional<bool> GifAutoPlay { get; set; }

        [JsonProperty("friend_source_flags")]
        public Optional<FriendSourceFlagsModel> FriendSourceFlags { get; set; }

        [JsonProperty("explicit_content_filter")]
        public Optional<UserContentFilterLevel> ExplicitContentFilter { get; set; }

        [JsonProperty("enable_tts_command")]
        public Optional<bool> EnableTtsCommand { get; set; }

        [JsonProperty("disable_games_tab")]
        public Optional<bool> DisableGamesTab { get; set; }

        [JsonProperty("developer_mode")]
        public Optional<bool> DeveloperMode { get; set; }

        [JsonProperty("detect_platform_accounts")]
        public Optional<bool> DetectPlatformAccounts { get; set; }

        [JsonProperty("default_guilds_restricted")]
        public Optional<bool> DefaultGuildsRestricted { get; set; }

        // TODO
        //[JsonProperty("custom_status")]
        //public object CustomStatus { get; set; }

        [JsonProperty("convert_emoticons")]
        public Optional<bool> ConvertEmoticons { get; set; }

        [JsonProperty("animate_emoji")]
        public Optional<bool> AnimateEmoji { get; set; }

        [JsonProperty("afk_timeout")]
        public Optional<long> AfkTimeout { get; set; }
    }
}
