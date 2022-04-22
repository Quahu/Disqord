using System;
using Disqord.Serialization.Json;
using Qommon;

namespace Disqord.Gateway.Api.Models
{
    public class ChannelPinsUpdateJsonModel : JsonModel
    {
        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("channel_id")]
        public Snowflake ChannelId;

        [JsonProperty("last_pin_timestamp")]
        public Optional<DateTimeOffset?> LastPinTimestamp;
    }
}
