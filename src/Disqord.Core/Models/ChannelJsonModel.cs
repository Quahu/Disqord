using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models;

public class ChannelJsonModel : JsonModel
{
    [JsonProperty("id")]
    public Snowflake Id;

    [JsonProperty("type")]
    public ChannelType Type;

    [JsonProperty("guild_id")]
    public Optional<Snowflake> GuildId;

    [JsonProperty("position")]
    public Optional<int> Position;

    [JsonProperty("permission_overwrites")]
    public Optional<OverwriteJsonModel[]> PermissionOverwrites;

    [JsonProperty("name")]
    public Optional<string> Name;

    [JsonProperty("topic")]
    public Optional<string> Topic;

    [JsonProperty("nsfw")]
    public Optional<bool> Nsfw;

    [JsonProperty("last_message_id")]
    public Optional<Snowflake?> LastMessageId;

    [JsonProperty("bitrate")]
    public Optional<int> Bitrate;

    [JsonProperty("user_limit")]
    public Optional<int> UserLimit;

    [JsonProperty("rate_limit_per_user")]
    public Optional<int> RateLimitPerUser;

    [JsonProperty("recipients")]
    public Optional<UserJsonModel[]> Recipients;

    [JsonProperty("icon")]
    public Optional<string> Icon;

    [JsonProperty("owner_id")]
    public Optional<Snowflake> OwnerId;

    [JsonProperty("application_id")]
    public Optional<Snowflake> ApplicationId;

    [JsonProperty("parent_id")]
    public Optional<Snowflake?> ParentId;

    [JsonProperty("last_pin_timestamp")]
    public Optional<DateTimeOffset?> LastPinTimestamp;

    [JsonProperty("rtc_region")]
    public Optional<string> RtcRegion;

    [JsonProperty("video_quality_mode")]
    public Optional<VideoQualityMode> VideoQualityMode;

    [JsonProperty("message_count")]
    public Optional<int> MessageCount;

    [JsonProperty("member_count")]
    public Optional<int> MemberCount;

    [JsonProperty("thread_metadata")]
    public Optional<ThreadMetadataJsonModel> ThreadMetadata;

    [JsonProperty("member")]
    public Optional<ThreadMemberJsonModel> Member;

    /// <summary>
    ///     Default duration for newly created threads, in minutes, to automatically archive the thread after recent activity
    /// </summary>
    [JsonProperty("default_auto_archive_duration")]
    public Optional<int> DefaultAutoArchiveDuration;

    [JsonProperty("permissions")]
    public Optional<Permissions> Permissions;

    [JsonProperty("flags")]
    public Optional<GuildChannelFlags> Flags;

    [JsonProperty("available_tags")]
    public Optional<ForumTagJsonModel[]> AvailableTags;

    [JsonProperty("applied_tags")]
    public Optional<Snowflake[]> AppliedTags;

    [JsonProperty("default_reaction_emoji")]
    public Optional<ForumDefaultReactionJsonModel> DefaultReactionEmoji;

    /// <summary>
    ///     Default slowmode for newly created threads, in seconds
    /// </summary>
    [JsonProperty("default_thread_rate_limit_per_user")]
    public Optional<int> DefaultThreadRateLimitPerUser;
}
