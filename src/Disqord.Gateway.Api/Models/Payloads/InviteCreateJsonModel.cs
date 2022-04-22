using System;
using Disqord.Models;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models
{
    public class InviteCreateJsonModel : JsonModel
    {
        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("code")]
        public string Code;

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("inviter")]
        public Optional<UserJsonModel> Inviter;

        [JsonProperty("max_age")]
        public int MaxAge;

        [JsonProperty("max_uses")]
        public int MaxUses;

        [JsonProperty("target_type")]
        public Optional<InviteTargetType> TargetType;

        [JsonProperty("target_user")]
        public Optional<UserJsonModel> TargetUser;

        [JsonProperty("target_application")]
        public Optional<ApplicationJsonModel> TargetApplication;

        [JsonProperty("temporary")]
        public bool Temporary;

        [JsonProperty("uses")]
        public int Uses;
    }
}
