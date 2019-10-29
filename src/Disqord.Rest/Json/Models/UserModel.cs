using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal class UserModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("username")]
        public Optional<string> Username { get; set; }

        [JsonProperty("discriminator")]
        public Optional<string> Discriminator { get; set; }

        [JsonProperty("avatar")]
        public Optional<string> Avatar { get; set; }

        [JsonProperty("bot")]
        public bool Bot { get; set; }

        [JsonProperty("mfa_enabled")]
        public Optional<bool> MfaEnabled { get; set; }

        [JsonProperty("locale")]
        public Optional<string> Locale { get; set; }

        [JsonProperty("verified")]
        public Optional<bool> Verified { get; set; }

        [JsonProperty("email")]
        public Optional<string> Email { get; set; }

        [JsonProperty("phone")]
        public Optional<string> Phone { get; set; }

        [JsonProperty("flags")]
        public Optional<UserFlags> Flags { get; set; }

        [JsonProperty("premium")]
        public Optional<bool> Premium { get; set; }

        [JsonProperty("premium_type")]
        public Optional<NitroType?> PremiumType { get; set; }
    }
}
