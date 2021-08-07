using Disqord.Serialization.Json;

namespace Disqord.Models
{
    public class ApplicationJsonModel : JsonModel
    {
        [JsonProperty("id")]
        public Snowflake Id;

        [JsonProperty("name")]
        public string Name;

        [JsonProperty("icon")]
        public string Icon;

        [JsonProperty("description")]
        public string Description;

        [JsonProperty("rpc_origins")]
        public Optional<string[]> RpcOrigins;

        [JsonProperty("bot_public")]
        public bool BotPublic;

        [JsonProperty("bot_require_code_grant")]
        public bool BotRequireCodeGrant;

        [JsonProperty("terms_of_service_url")]
        public Optional<string> TermsOfServiceUrl;

        [JsonProperty("privacy_policy_url")]
        public Optional<string> PrivacyPolicyUrl;

        [JsonProperty("owner")]
        public Optional<UserJsonModel> Owner;

        [JsonProperty("summary")]
        public string Summary;

        [JsonProperty("verify_key")]
        public string VerifyKey;

        [JsonProperty("team")]
        public TeamJsonModel Team;

        [JsonProperty("guild_id")]
        public Optional<Snowflake> GuildId;

        [JsonProperty("primary_sku_id")]
        public Optional<Snowflake> PrimarySkuId;

        [JsonProperty("slug")]
        public Optional<string> Slug;

        [JsonProperty("cover_image")]
        public Optional<string> CoverImage;

        [JsonProperty("flags")]
        public Optional<int> Flags;
    }
}
