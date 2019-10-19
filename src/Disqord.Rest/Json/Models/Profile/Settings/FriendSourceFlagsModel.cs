using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class FriendSourceFlagsModel
    {
        [JsonProperty("mutual_guilds")]
        public Optional<bool> MutualGuilds { get; set; }

        [JsonProperty("mutual_friends")]
        public Optional<bool> MutualFriends { get; set; }

        [JsonProperty("all")]
        public Optional<bool> All { get; set; }
    }
}
