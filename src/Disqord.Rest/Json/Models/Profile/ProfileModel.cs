using System;
using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class ProfileModel
    {
        [JsonProperty("premium_since")]
        public DateTimeOffset? PremiumSince { get; set; }

        [JsonProperty("premium_guild_since")]
        public DateTimeOffset? PremiumGuildSince { get; set; }

        [JsonProperty("mutual_guilds")]
        public MutualGuildModel[] MutualGuilds { get; set; }

        [JsonProperty("user")]
        public UserModel User { get; set; }

        [JsonProperty("connected_accounts")]
        public ConnectedAccountModel[] ConnectedAccounts { get; set; }
    }
}
