using System;
using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class InviteCreateModel
    {
        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("temporary")]
        public bool Temporary { get; set; }

        [JsonProperty("max_uses")]
        public int MaxUses { get; set; }

        [JsonProperty("max_age")]
        public int MaxAge { get; set; }

        [JsonProperty("inviter")]
        public Optional<UserModel> Inviter { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }
    }
}
