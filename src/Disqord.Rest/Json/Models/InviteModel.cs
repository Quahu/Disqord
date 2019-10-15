using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class InviteModel
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("guild")]
        public GuildModel Guild { get; set; }

        [JsonProperty("channel")]
        public ChannelModel Channel { get; set; }

        [JsonProperty("approximate_presence_count")]
        public int? ApproximatePresenceCount { get; set; }

        [JsonProperty("approximate_member_count")]
        public int? ApproximateMemberCount { get; set; }

        // metadata
        [JsonProperty("inviter")]
        public UserModel Inviter { get; set; }

        [JsonProperty("uses")]
        public int Uses { get; set; }

        [JsonProperty("max_uses")]
        public int MaxUses { get; set; }

        [JsonProperty("max_age")]
        public int MaxAge { get; set; }

        [JsonProperty("temporary")]
        public bool Temporary { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("revoked")]
        public bool Revoked { get; set; }
    }
}
