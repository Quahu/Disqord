using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

[JsonSkippedProperties("nsfw")]
public class GuildJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("name")]
    public string Name = null!;

    [JsonProperty("icon")]
    public string? Icon;

    [JsonProperty("icon_hash")]
    public Optional<string?> IconHash;

    [JsonProperty("splash")]
    public string? Splash;

    [JsonProperty("discovery_splash")]
    public Optional<string?> DiscoverySplash;

    [JsonProperty("owner")]
    public Optional<bool> Owner;

    [JsonProperty("owner_id")]
    public Snowflake OwnerId;

    [JsonProperty("permissions")]
    public Optional<Permissions> Permissions;

    [JsonProperty("afk_channel_id")]
    public Snowflake? AfkChannelId;

    [JsonProperty("afk_timeout")]
    public int AfkTimeout;

    [JsonProperty("widget_enabled")]
    public Optional<bool> WidgetEnabled;

    [JsonProperty("widget_channel_id")]
    public Optional<Snowflake?> WidgetChannelId;

    [JsonProperty("verification_level")]
    public GuildVerificationLevel VerificationLevel;

    [JsonProperty("default_message_notifications")]
    public GuildNotificationLevel DefaultMessageNotifications;

    [JsonProperty("explicit_content_filter")]
    public GuildContentFilterLevel ExplicitContentFilter;

    [JsonProperty("roles")]
    public RoleJsonModel[] Roles = null!;

    [JsonProperty("emojis")]
    public EmojiJsonModel[] Emojis = null!;

    [JsonProperty("features")]
    public string[] Features = null!;

    [JsonProperty("mfa_level")]
    public GuildMfaLevel MfaLevel;

    [JsonProperty("application_id")]
    public Snowflake? ApplicationId;

    [JsonProperty("system_channel_id")]
    public Snowflake? SystemChannelId;

    [JsonProperty("system_channel_flags")]
    public SystemChannelFlags SystemChannelFlags;

    [JsonProperty("rules_channel_id")]
    public Snowflake? RulesChannelId;

    [JsonProperty("max_presences")]
    public Optional<int?> MaxPresences;

    [JsonProperty("max_members")]
    public Optional<int> MaxMembers;

    [JsonProperty("vanity_url_code")]
    public string? VanityUrlCode;

    [JsonProperty("description")]
    public string? Description;

    [JsonProperty("banner")]
    public string? Banner;

    [JsonProperty("premium_tier")]
    public GuildBoostTier PremiumTier;

    [JsonProperty("premium_subscription_count")]
    public Optional<int> PremiumSubscriptionCount;

    [JsonProperty("preferred_locale")]
    public string PreferredLocale = null!;

    [JsonProperty("public_updates_channel_id")]
    public Snowflake? PublicUpdatesChannelId;

    [JsonProperty("max_video_channel_users")]
    public Optional<int> MaxVideoChannelUsers;

    [JsonProperty("approximate_member_count")]
    public Optional<int> ApproximateMemberCount;

    [JsonProperty("approximate_presence_count")]
    public Optional<int> ApproximatePresenceCount;

    [JsonProperty("nsfw_level")]
    public GuildNsfwLevel NsfwLevel;

    [JsonProperty("stickers")]
    public Optional<StickerJsonModel[]> Stickers;

    [JsonProperty("welcome_screen")]
    public Optional<WelcomeScreenJsonModel> WelcomeScreen;

    [JsonProperty("premium_progress_bar_enabled")]
    public bool PremiumProgressBarEnabled;
}
