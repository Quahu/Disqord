using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Models
{
    public class InviteJsonModel : JsonModel
    {
        [JsonProperty("code")]
        public string Code;

        [JsonProperty("guild")]
        public Optional<GuildJsonModel> Guild;

        [JsonProperty("channel")]
        public ChannelJsonModel Channel;

        [JsonProperty("inviter")]
        public Optional<UserJsonModel> Inviter;

        [JsonProperty("target_type")]
        public Optional<InviteTargetType> TargetType;

        [JsonProperty("target_user")]
        public Optional<UserJsonModel> TargetUser;

        [JsonProperty("target_application")]
        public Optional<ApplicationJsonModel> TargetApplication;

        [JsonProperty("approximate_presence_count")]
        public Optional<int> ApproximatePresenceCount;

        [JsonProperty("approximate_member_count")]
        public Optional<int> ApproximateMemberCount;

        [JsonProperty("expires_at")]
        public Optional<DateTimeOffset?> ExpiresAt;

        [JsonProperty("stage_instance")]
        public Optional<InviteStageInstanceJsonModel> StageInstance;

        [JsonProperty("guild_scheduled_event")]
        public Optional<GuildScheduledEventJsonModel> GuildScheduledEvent;

        // Metadata
        [JsonProperty("uses")]
        public Optional<int> Uses;

        [JsonProperty("max_uses")]
        public Optional<int> MaxUses;

        [JsonProperty("max_age")]
        public Optional<int> MaxAge;

        [JsonProperty("temporary")]
        public Optional<bool> Temporary;

        [JsonProperty("created_at")]
        public Optional<DateTimeOffset> CreatedAt;
    }
}
