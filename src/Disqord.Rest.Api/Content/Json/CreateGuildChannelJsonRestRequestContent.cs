using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Rest.Api
{
    public class CreateGuildChannelJsonRestRequestContent : JsonModelRestRequestContent
    {
        [JsonProperty("name")]
        public string Name;

        [JsonProperty("type")]
        public Optional<ChannelType> Type;

        [JsonProperty("topic")]
        public Optional<string> Topic;

        [JsonProperty("bitrate")]
        public Optional<int> Bitrate;

        [JsonProperty("user_limit")]
        public Optional<int> UserLimit;

        [JsonProperty("rate_limit_per_user")]
        public Optional<int> RateLimitPerUser;

        [JsonProperty("position")]
        public Optional<int> Position;

        [JsonProperty("permission_overwrites")]
        public Optional<OverwriteJsonModel[]> PermissionOverwrites;

        [JsonProperty("parent_id")]
        public Optional<Snowflake> ParentId;

        [JsonProperty("nsfw")]
        public Optional<bool> Nsfw;

        [JsonProperty("default_auto_archive_duration")]
        public Optional<int> DefaultAutoArchiveDuration;

        [JsonProperty("rtc_region")]
        public Optional<string> RtcRegion;

        public CreateGuildChannelJsonRestRequestContent(string name)
        {
            Name = name;
        }
    }
}
