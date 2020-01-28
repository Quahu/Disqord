using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class ChannelModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("type")]
        public Optional<ChannelType> Type { get; set; }

        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }

        [JsonProperty("position")]
        public Optional<int> Position { get; set; }

        [JsonProperty("permission_overwrites")]
        public Optional<OverwriteModel[]> PermissionOverwrites { get; set; }

        [JsonProperty("name")]
        public Optional<string> Name { get; set; }

        [JsonProperty("topic")]
        public Optional<string> Topic { get; set; }

        [JsonProperty("nsfw")]
        public Optional<bool> Nsfw { get; set; }

        [JsonProperty("last_message_id")]
        public Optional<ulong?> LastMessageId { get; set; }

        [JsonProperty("bitrate")]
        public Optional<int> Bitrate { get; set; }

        [JsonProperty("user_limit")]
        public Optional<int> UserLimit { get; set; }

        [JsonProperty("rate_limit_per_user")]
        public Optional<int> RateLimitPerUser { get; set; }

        [JsonProperty("recipients")]
        public Optional<UserModel[]> Recipients { get; set; }

        [JsonProperty("icon")]
        public Optional<string> Icon { get; set; }

        [JsonProperty("owner_id")]
        public Optional<ulong> OwnerId { get; set; }

        [JsonProperty("application_id")]
        public Optional<ulong> ApplicationId { get; set; }

        [JsonProperty("parent_id")]
        public Optional<ulong?> ParentId { get; set; }

        [JsonProperty("last_pin_timestamp")]
        public Optional<DateTimeOffset?> LastPinTimestamp { get; set; }
    }
}
