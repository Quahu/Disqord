using Disqord.Serialization.Json;

namespace Disqord.Rest
{
    internal sealed class SendFriendRequestContent : JsonRequestContent
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("discriminator")]
        public string Discriminator { get; set; }
    }
}
