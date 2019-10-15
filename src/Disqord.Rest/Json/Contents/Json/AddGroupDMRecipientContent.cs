using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class AddGroupDmRecipientContent : JsonRequestContent
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("nick")]
        public string Nick { get; set; }
    }
}
