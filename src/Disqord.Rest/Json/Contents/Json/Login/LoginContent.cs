using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class LoginContent : JsonRequestContent
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("undelete")]
        public bool Undelete { get; set; }

        [JsonProperty("captcha_key")]
        public string CaptchaKey { get; set; }

        [JsonProperty("login_source")]
        public string LoginSource { get; set; }

        [JsonProperty("gift_code_sku_id")]
        public string GiftCodeSkuId { get; set; }
    }
}
