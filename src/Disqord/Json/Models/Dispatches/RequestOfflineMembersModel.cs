using Disqord.Serialization.Json;

namespace Disqord.Models.Dispatches
{
    internal sealed class RequestOfflineMembersModel
    {
        /// <summary>
        ///     A snowflake or an array of snowflakes.
        /// </summary>
        [JsonProperty("guild_id")]
        public object GuildId { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("presences")]
        public bool Presences { get; set; }
    }
}
