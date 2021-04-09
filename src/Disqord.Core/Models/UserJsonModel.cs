using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class UserJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("username")]
        public string Username;

        [JsonProperty("discriminator")]
        public string Discriminator;

        [JsonProperty("avatar")]
        public string Avatar;

        [JsonProperty("bot")]
        public Optional<bool> Bot;

        [JsonProperty("system")]
        public Optional<bool> System;

        [JsonProperty("mfa_enabled")]
        public Optional<bool> MfaEnabled;

        [JsonProperty("locale")]
        public Optional<string> Locale;

        [JsonProperty("verified")]
        public Optional<bool> Verified;

        [JsonProperty("email")]
        public Optional<string> Email;

        [JsonProperty("flags")]
        public Optional<UserFlag> Flags;

        [JsonProperty("premium_type")]
        public Optional<NitroType> PremiumType;

        [JsonProperty("public_flags")]
        public Optional<UserFlag> PublicFlags;
    }
}
