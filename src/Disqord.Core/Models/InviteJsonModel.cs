using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class InviteJsonModel : JsonModel
    {
        [JsonProperty("code")]
        public string Code = default!;

        [JsonProperty("guild")]
        public Optional<GuildJsonModel> Guild;

        [JsonProperty("channel")]
        public ChannelJsonModel Channel = default!;

        [JsonProperty("inviter")]
        public Optional<UserJsonModel> Inviter;

        [JsonProperty("target_user")]
        public Optional<UserJsonModel> TargetUser;

        [JsonProperty("target_user_type")]
        public Optional<int> TargetUserType;

        [JsonProperty("approximate_presence_count")]
        public Optional<int> ApproximatePresenceCount;

        [JsonProperty("approximate_member_count")]
        public Optional<int> ApproximateMemberCount;

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
