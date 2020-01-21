using Disqord.Serialization.Json;

namespace Disqord.Models
{
    internal sealed class LoginModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("mfa")]
        public bool Mfa { get; set; }

        [JsonProperty("sms")]
        public bool Sms { get; set; }

        [JsonProperty("ticket")]
        public string Ticket { get; set; }
    }
}
