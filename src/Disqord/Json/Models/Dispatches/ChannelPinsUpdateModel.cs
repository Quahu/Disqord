using System;
using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class ChannelPinsUpdateModel
    {
        [JsonProperty("guild_id")]
        public ulong? GuildId { get; set; }

        [JsonProperty("channel_id")]
        public ulong ChannelId { get; set; }

        [JsonProperty("last_pin_timestamp")]
        public DateTimeOffset? LastPinTimestamp { get; set; }
    }
}
