using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class TotpContent : JsonRequestContent
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("ticket")]
        public string Ticket { get; set; }

        [JsonProperty("login_source")]
        public string LoginSource { get; set; }

        [JsonProperty("gift_code_sku_id")]
        public string GiftCodeSkuId { get; set; }
    }
}
