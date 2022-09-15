using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api;

public class ModifyChannelJsonRestRequestContent : JsonModelRestRequestContent
{
    [JsonProperty("name")]
    public Optional<string> Name;

    [JsonProperty("type")]
    public Optional<ChannelType> Type;

    [JsonProperty("position")]
    public Optional<int> Position;

    [JsonProperty("topic")]
    public Optional<string> Topic;

    [JsonProperty("nsfw")]
    public Optional<bool> Nsfw;

    [JsonProperty("rate_limit_per_user")]
    public Optional<int> RateLimitPerUser;

    [JsonProperty("bitrate")]
    public Optional<int> Bitrate;

    [JsonProperty("user_limit")]
    public Optional<int> UserLimit;

    [JsonProperty("permission_overwrites")]
    public Optional<OverwriteJsonModel[]> PermissionOverwrites;

    [JsonProperty("parent_id")]
    public Optional<Snowflake?> ParentId;

    [JsonProperty("video_quality_mode")]
    public Optional<VideoQualityMode> VideoQualityMode;

    [JsonProperty("default_auto_archive_duration")]
    public Optional<int> DefaultAutoArchiveDuration;

    [JsonProperty("rtc_region")]
    public Optional<string> RtcRegion;

    [JsonProperty("archived")]
    public Optional<bool> Archived;

    [JsonProperty("auto_archive_duration")]
    public Optional<int> AutoArchiveDuration;

    [JsonProperty("locked")]
    public Optional<bool> Locked;

    [JsonProperty("invitable")]
    public Optional<bool> Invitable;

    [JsonProperty("flags")]
    public Optional<GuildChannelFlags> Flags;

    [JsonProperty("available_tags")]
    public Optional<ForumTagJsonModel[]> AvailableTags;

    [JsonProperty("default_reaction_emoji")]
    public Optional<ForumDefaultReactionJsonModel> DefaultReactionEmoji;

    [JsonProperty("default_thread_rate_limit_per_user")]
    public Optional<int> DefaultThreadRateLimitPerUser;

    [JsonProperty("applied_tags")]
    public Optional<Snowflake[]> AppliedTags;
}
